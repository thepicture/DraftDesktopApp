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

            FilterTypes = new List<MaterialType>
            {
                new MaterialType
                {
                    Title="Все типы"
                },
            };

            _ = LoadFilters()
                .ContinueWith(t => LoadSortTypes())
                .ContinueWith(t => LoadMaterials());

        }

        private void LoadSortTypes()
        {
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
        }

        private async Task LoadFilters()
        {
            List<MaterialType> materialTypes =
                await _context.MaterialType.ToListAsync();
            FilterTypes
                .AddRange(materialTypes);
            CurrentFilterType = FilterTypes.First();
        }

        private async void LoadMaterials()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            List<Material> currentMaterials = await _context.Material.ToListAsync();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                currentMaterials = currentMaterials.Where(m =>
                {
                    string lowerCaseSearchText = SearchText.ToLower();
                    return m.Title
                    .ToLower()
                    .Contains(lowerCaseSearchText);
                })
                    .ToList();
            }
            if (currentFilterType != null
                && CurrentFilterType.Title != "Все типы")
            {
                currentMaterials = currentMaterials.Where(m => m.MaterialType.ID == CurrentFilterType.ID)
                    .ToList();
            }

            if (CurrentSortType != null
                && CurrentSortType != "Сортировка")
            {
                switch (CurrentSortType)
                {
                    case "Наименование по возрастанию":
                        currentMaterials = currentMaterials.OrderBy(m => m.Title)
                            .ToList();
                        break;
                    case "Наименование по убыванию":
                        currentMaterials = currentMaterials.OrderByDescending(m => m.Title)
                            .ToList();
                        break;
                    case "Остаток на складе по возрастанию":
                        currentMaterials = currentMaterials.OrderBy(m => m.CountInStock)
                            .ToList();
                        break;
                    case "Остаток на складе по убыванию":
                        currentMaterials = currentMaterials.OrderByDescending(m => m.CountInStock)
                            .ToList();
                        break;
                    case "Стоимость по возрастанию":
                        currentMaterials = currentMaterials.OrderBy(m => m.Cost)
                            .ToList();
                        break;
                    case "Стоимость по убыванию":
                        currentMaterials = currentMaterials.OrderByDescending(m => m.Cost)
                            .ToList();
                        break;
                    default:
                        break;
                }
            }
            Materials = currentMaterials;
            IsBusy = false;
        }

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    LoadMaterials();
                }
            }
        }

        private List<MaterialType> _filterTypes;
        private IEnumerable<string> _sortTypes;

        private string _currentSortType;

        public string CurrentSortType
        {
            get => _currentSortType;
            set
            {
                if (SetProperty(ref _currentSortType, value))
                {
                    LoadMaterials();
                }
            }
        }

        private MaterialType currentFilterType;

        public MaterialType CurrentFilterType
        {
            get => currentFilterType;
            set
            {
                if (SetProperty(ref currentFilterType, value))
                {
                    LoadMaterials();
                }
            }
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
