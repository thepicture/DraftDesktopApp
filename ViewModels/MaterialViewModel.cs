using DraftDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DraftDesktopApp.ViewModels
{
    public class MaterialViewModel : ViewModelBase
    {
        private readonly DraftBaseEntities _context =
            new DraftBaseEntities();
        public MaterialViewModel()
        {
            Title = "Материалы";

            SortTypes = new List<string>
            {
                "Сортировка",
                "Наименование по возрастанию",
                "Наименование по убыванию",
                "Остаток на складе по возрастанию",
                "Остаток на складе по убыванию",
                "Стоимость по возрастанию",
                "Стоимость по убыванию",
            };

            CurrentSortType = SortTypes.First();

            _ = LoadFilters()
                .ContinueWith(t => LoadMaterials());
        }

        private async Task LoadFilters()
        {
            FilterTypes = new List<MaterialType>
            {
                new MaterialType {Title="Все типы"},
            };
            List<MaterialType> materialTypes =
                await _context.MaterialType.ToListAsync();
            FilterTypes
                .AddRange(materialTypes);
            CurrentFilterType = FilterTypes.First();
        }

        private async void LoadMaterials()
        {
            Materials = await _context.Material.ToListAsync();
        }

        private string searchText;

        public string SearchText
        {
            get => searchText = string.Empty;
            set => SetProperty(ref searchText, value);
        }

        private List<MaterialType> _filterTypes;
        private IEnumerable<string> _sortTypes;

        private string currentSortType;

        public string CurrentSortType
        {
            get => currentSortType;
            set => SetProperty(ref currentSortType, value);
        }

        private MaterialType currentFilterType;

        public MaterialType CurrentFilterType
        {
            get => currentFilterType;
            set => SetProperty(ref currentFilterType, value);
        }
        public List<MaterialType> FilterTypes
        {
            get => _filterTypes;
            set => SetProperty(ref _filterTypes, value);
        }
        public IEnumerable<string> SortTypes
        {
            get => _sortTypes;
            set => SetProperty(ref _sortTypes, value);
        }
        public IEnumerable<Material> Materials
        {
            get => _materials;
            set => SetProperty(ref _materials, value);
        }

        private IEnumerable<Material> _materials;
    }
}
