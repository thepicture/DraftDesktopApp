using DraftDesktopApp.Commands;
using DraftDesktopApp.Models.Entities;
using DraftDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace DraftDesktopApp.ViewModels
{
    public class AddEditMaterialViewModel : ViewModelBase
    {
        private Material _currentMaterial;
        private IList<MaterialType> _materialTypes;
        private MaterialType _currentType;
        private readonly DraftBaseEntities _context = new DraftBaseEntities();
        public AddEditMaterialViewModel()
        {
            CurrentMaterial = new Material();
            MaterialTypes = _context.MaterialType.ToList();
            CurrentType = MaterialTypes.First();

            SupplierPositions = _context.Supplier.ToList();
            CurrentPosition = SupplierPositions.FirstOrDefault();
        }

        public AddEditMaterialViewModel(Material material)
        {
            CurrentMaterial = _context.Material.Find(material.ID);
            MaterialTypes = _context.MaterialType.ToList();
            CurrentType = MaterialTypes.FirstOrDefault(t => t.ID == material.MaterialTypeID);
            if (material.Supplier.Count() > 0)
            {
                UpdateSuppliers();

                SupplierPositions = _context.Supplier
                    .ToList()
                    .Where(p =>
                    {
                        return !CurrentMaterial.Supplier
                        .Select(s => s.ID)
                        .Contains(p.ID);
                    })
                    .ToList();
                CurrentPosition = SupplierPositions.FirstOrDefault();
            }
        }

        public Material CurrentMaterial
        {
            get => _currentMaterial;
            set => SetProperty(ref _currentMaterial, value);
        }
        public IList<MaterialType> MaterialTypes
        {
            get => _materialTypes;
            set => SetProperty(ref _materialTypes, value);
        }
        public MaterialType CurrentType
        {
            get => _currentType;
            set => SetProperty(ref _currentType, value);
        }
        public RelayCommand SaveChangesCommand
        {
            get
            {
                if (_saveChangesCommand == null)
                {
                    _saveChangesCommand = new RelayCommand(PerformSaveChanges, CanSaveChangesExecute);
                }
                return _saveChangesCommand;
            }
            set => _saveChangesCommand = value;
        }

        private bool CanSaveChangesExecute(object arg)
        {
            StringBuilder errors = new StringBuilder();
            if (!new Regex("[0-9]+(.[0-9]{1,2})?").IsMatch(CurrentMaterial.Cost.ToString())
                || CurrentMaterial.Cost < 0)
            {
                _ = errors.AppendLine("Стоимость материала не может быть " +
                    "отрицательной и записывается только с точностью " +
                    "до сотых");
            }
            if (CurrentMaterial.MinCount < 0)
            {
                _ = errors.AppendLine("Минимальное количество " +
                    "не может быть отрицательным");
            }
            if (CurrentMaterial.CountInStock < 0)
            {
                _ = errors.AppendLine("Количество на складе " +
                    "не может быть отрицательным");
            }
            if (CurrentMaterial.CountInPack < 0)
            {
                _ = errors.AppendLine("Минимальное количество " +
                    "не может быть отрицательным");
            }
            if (string.IsNullOrEmpty(CurrentMaterial.Unit))
            {
                _ = errors.AppendLine("Единица измерения - обязательное поле");
            }
            ValidationText = errors.ToString();
            if (string.IsNullOrEmpty(ValidationText))
            {
                CalculateMinimumMaterialBuyCount();
                return true;
            }
            else
            {
                MinimumBuyMaterialCountText = string.Empty;
                return false;
            }
        }

        private void CalculateMinimumMaterialBuyCount()
        {
            if (CurrentMaterial.CountInStock < CurrentMaterial.MinCount)
            {
                double countDifference =
                    (double)(CurrentMaterial.MinCount
                    - CurrentMaterial.CountInStock);
                int packsToBuy = (int)Math.Ceiling(countDifference
                    * 1.0
                    / CurrentMaterial.CountInPack);
                MinimumBuyMaterialCountText = $"Необходимо купить " +
                    $"упаковок: {packsToBuy}.\nОбщая сумма за покупку: " +
                    $"{CurrentMaterial.Cost * packsToBuy} рублей";
            }
            else
            {
                MinimumBuyMaterialCountText = "Закупка не нужна. Материалы есть на складе";
            }
        }

        public RelayCommand DeleteMaterialCommand
        {
            get
            {
                if (_deleteMaterialCommand == null)
                {
                    _deleteMaterialCommand = new RelayCommand(PerformDelete);
                }
                return _deleteMaterialCommand;
            }

            set => _deleteMaterialCommand = value;
        }

        private void PerformDelete(object obj)
        {
            if (!FeedbackService.AskQuestion("Вы действительно хотите удалить материал?"))
            {
                FeedbackService.ShowInfo("Удаление материала отменено");
                return;
            }
            if (CurrentMaterial.ProductMaterial.Count() > 0)
            {
                return;
            }
            CurrentMaterial.Supplier.Clear();
            CurrentMaterial.MaterialCountHistory.Clear();

            _context.Material.Remove(CurrentMaterial);

            if (IsSavedChanges())
            {
                NavigationService.Navigate<MaterialViewModel>();
            }
        }

        private bool IsSavedChanges()
        {
            try
            {
                _ = _context.SaveChanges();
                FeedbackService.ShowInfo("Изменения успешно сохранены!");
                return true;
            }
            catch (Exception ex)
            {
                FeedbackService.ShowInfo("Не удалось изменить материал. " +
                    "Вероятно, поля заполнены неверно. Попробуйте " +
                    "перезайти на страницу и попробовать ещё раз. " +
                    "Если это не поможет, перезапустите приложение. " +
                    "Дальнейшие действия - обратитесь к администратору");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private void PerformSaveChanges(object obj)
        {
            CurrentMaterial.MaterialType = CurrentType;
            if (CurrentMaterial.ID == 0)
            {
                _ = _context.Material.Add(CurrentMaterial);
            }
            if (IsSavedChanges())
            {
                NavigationService.Navigate<MaterialViewModel>();
            }
        }

        private RelayCommand _saveChangesCommand;

        private RelayCommand _deleteMaterialCommand;

        private string _supplierSearchText = string.Empty;

        public string SupplierSearchText
        {
            get => _supplierSearchText;
            set
            {
                if (SetProperty(ref _supplierSearchText, value))
                {
                    UpdateSuppliers();
                }
            }
        }

        private void UpdateSuppliers()
        {
            MaterialSuppliers = CurrentMaterial.Supplier
                .Where(s =>
                {
                    string searchText = SupplierSearchText.ToLower();
                    return s.Title
                    .ToLower()
                    .Contains(searchText);
                });
        }

        private IEnumerable<Supplier> _materialSuppliers;

        public IEnumerable<Supplier> MaterialSuppliers
        {
            get => _materialSuppliers;
            set => SetProperty(ref _materialSuppliers, value);
        }

        private IList<Supplier> _supplierPositions = new List<Supplier>();

        public IList<Supplier> SupplierPositions
        {
            get => _supplierPositions;
            set => SetProperty(ref _supplierPositions, value);
        }

        private RelayCommand _addPositionCommand;

        public ICommand AddPositionCommand
        {
            get
            {
                if (_addPositionCommand == null)
                {
                    _addPositionCommand = new RelayCommand(AddPosition,
                                                           CanAddPositionExecute);
                }

                return _addPositionCommand;
            }
        }

        private bool CanAddPositionExecute(object arg)
        {
            return SupplierPositions.Count() > 0;
        }

        private void AddPosition(object commandParameter)
        {
            if (!FeedbackService.AskQuestion("Вы действительно хотите добавить позицию?"))
            {
                FeedbackService.ShowInfo("Добавление позиции отменено");
                return;
            }
            CurrentMaterial.Supplier.Add(CurrentPosition);
            UpdateSuppliers();
            SupplierPositions = SupplierPositions
                .Where(p => p.ID != CurrentPosition.ID)
                .ToList();
            CurrentPosition = SupplierPositions.FirstOrDefault();
        }

        private Supplier _currentPosition;

        public Supplier CurrentPosition
        {
            get => _currentPosition;
            set => SetProperty(ref _currentPosition, value);
        }

        private RelayCommand deletePositionCommand;

        public ICommand DeletePositionCommand
        {
            get
            {
                if (deletePositionCommand == null)
                {
                    deletePositionCommand = new RelayCommand(DeletePosition);
                }

                return deletePositionCommand;
            }
        }

        public string ValidationText
        {
            get => _validationText;
            set => SetProperty(ref _validationText, value);
        }
        public string MinimumBuyMaterialCountText
        {
            get => _minimumBuyMaterialCountText;
            set => SetProperty(ref _minimumBuyMaterialCountText, value);
        }

        private void DeletePosition(object commandParameter)
        {
            if (!FeedbackService.AskQuestion("Действительно удалить позицию?"))
            {
                FeedbackService.ShowInfo("Удаление позиции отменено");
                return;
            }
            Supplier selectedSupplier = commandParameter as Supplier;
            Supplier materialSupplier = CurrentMaterial.Supplier
                .First(s => s.ID == selectedSupplier.ID);
            if (CurrentMaterial.Supplier.Remove(materialSupplier))
            {
                UpdateSuppliers();
                SupplierPositions = _context.Supplier
                      .ToList()
                      .Where(p =>
                      {
                          return !CurrentMaterial.Supplier
                          .Select(s => s.ID)
                          .Contains(p.ID);
                      })
                      .ToList();
                CurrentPosition = SupplierPositions.FirstOrDefault();
            }
        }
        private string _validationText = string.Empty;
        private string _minimumBuyMaterialCountText = string.Empty;

        private RelayCommand changePictureCommand;

        public ICommand ChangePictureCommand
        {
            get
            {
                if (changePictureCommand == null)
                {
                    changePictureCommand = new RelayCommand(ChangePicture);
                }

                return changePictureCommand;
            }
        }

        private void ChangePicture(object commandParameter)
        {
            IFileSelector<byte[]> fileSelector = DependencyService
                .Get<IFileSelector<byte[]>>();
            string filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
            if (fileSelector.TryToSelect(out byte[] materialImage, filter))
            {
                CurrentMaterial.ImageBytes = materialImage;
                OnPropertyChanged(nameof(CurrentMaterial));
                FeedbackService.ShowInfo("Фото успешно изменено!");
            }
            else
            {
                FeedbackService.ShowInfo("Фото не прикреплено. " +
                    "Убедитесь, что вы выбрали изображение");
            }
        }
    }
}
