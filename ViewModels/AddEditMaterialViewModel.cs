using DraftDesktopApp.Models.Entities;
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
    }
}
