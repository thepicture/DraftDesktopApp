using DraftDesktopApp.Commands;
using DraftDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DraftDesktopApp.ViewModels
{
    public class MaterialMinimumCountViewModel : ViewModelBase
    {
        private Material _currentMaterial;
        private readonly DraftBaseEntities _context;
        private IEnumerable<Material> _materials;

        public MaterialMinimumCountViewModel(IEnumerable<Material> materials)
        {
            _context = new DraftBaseEntities();
            CurrentMinimumCount = materials.Max(m => m.MinCount).ToString();
            Title = "Изменить минимальное количество выбранных материалов";
            Materials = materials.ToList();
        }

        public Material CurrentMaterial
        {
            get => _currentMaterial;
            set => SetProperty(ref _currentMaterial, value);
        }

        private string _currentMinimumCount;

        public string CurrentMinimumCount
        {
            get => _currentMinimumCount;
            set
            {
                if (SetProperty(ref _currentMinimumCount, value))
                {
                    IsValid = double.TryParse(value, out _);
                }
            }
        }

        private RelayCommand changeMinCountCommand;

        public ICommand ChangeMinCountCommand
        {
            get
            {
                if (changeMinCountCommand == null)
                {
                    changeMinCountCommand = new RelayCommand(ChangeMinCount);
                }

                return changeMinCountCommand;
            }
        }

        public IEnumerable<Material> Materials
        {
            get => _materials;
            set => SetProperty(ref _materials, value);
        }

        private void ChangeMinCount(object commandParameter)
        {
            if (!FeedbackService.AskQuestion("Вы действительно хотите " +
                "изменить минимальное количество выбранных материалов?"))
            {
                FeedbackService.ShowInfo("Изменение отменено");
                return;
            }
            foreach (Material material in Materials)
            {
                _context.Material
                    .Find(material.ID)
                    .MinCount = double.Parse(CurrentMinimumCount);
            }

            try
            {
                _ = _context.SaveChanges();
                NavigationService.GoBack();
                FeedbackService.ShowInfo("Изменения успешно применены!");
            }
            catch (Exception ex)
            {
                FeedbackService.ShowError("Не удалось применить изменения. " +
                    "Перезайдите на страницу, если это не поможет, то " +
                    "перезайдите в приложение. В противном случае " +
                    "обратитесь к администратору");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
