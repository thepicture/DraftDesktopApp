using DraftDesktopApp.Commands;
using DraftDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DraftDesktopApp.ViewModels
{
    public class AddEditMaterialViewModel : ViewModelBase
    {
        private Material _currentMaterial;
        private IList<MaterialType> _materialTypes;
        private MaterialType _currentType;
        private DraftBaseEntities _context = new DraftBaseEntities();
        public AddEditMaterialViewModel()
        {
            CurrentMaterial = new Material();
            MaterialTypes = _context.MaterialType.ToList();
            CurrentType = MaterialTypes.First();
        }

        public AddEditMaterialViewModel(Material material)
        {
            CurrentMaterial = _context.Material.Find(material.ID);
            MaterialTypes = _context.MaterialType.ToList();
            CurrentType = MaterialTypes.FirstOrDefault(t => t.ID == material.MaterialTypeID);
            if (material.Supplier.Count() > 0)
            {
                UpdateSuppliers();
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
                    _saveChangesCommand = new RelayCommand(PerformSaveChanges);
                }
                return _saveChangesCommand;
            }
            set => _saveChangesCommand = value;
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
            if (CurrentMaterial.ProductMaterial.Count() > 0)
            {
                return;
            }

            if (CurrentMaterial.Supplier.Count() > 0)
            {
                _context.Supplier.RemoveRange(CurrentMaterial.Supplier);
            }

            if (CurrentMaterial.MaterialCountHistory.Count() > 0)
            {
                _context.MaterialCountHistory.RemoveRange(CurrentMaterial.MaterialCountHistory);
            }

            _context.Material.Remove(CurrentMaterial);

            SaveChanges();

            NavigationService.Navigate<MaterialViewModel>();
        }

        private void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                NavigationService.Navigate<MaterialViewModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void PerformSaveChanges(object obj)
        {
            CurrentMaterial.MaterialType = CurrentType;
            if (CurrentMaterial.ID == 0)
            {
                _context.Material.Add(CurrentMaterial);
            }
            SaveChanges();
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
    }
}
