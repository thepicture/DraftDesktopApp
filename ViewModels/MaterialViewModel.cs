using DraftDesktopApp.Commands;
using DraftDesktopApp.Models;
using DraftDesktopApp.Models.Entities;
using DraftDesktopApp.Services;
using System;
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

        private IList<PaginatorItem> _paginatorItems = new List<PaginatorItem>();

        private async void LoadMaterials()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            List<Material> currentMaterials = await _context.Material.ToListAsync();
            AllMaterialsCount = currentMaterials.Count();
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
            FoundMaterialsCount = Materials.Count();

            Materials = Materials.Skip(CurrentPage - 1).Take(15);

            LoadPages();
            IsBusy = false;
        }

        private void LoadPages()
        {
            var currentPageItems = new List<PaginatorItem>();
            for (int i = 1; i < (int)Math.Ceiling(FoundMaterialsCount * 1.0 / 15); i++)
            {
                currentPageItems.Add(new PaginatorItem(i, CurrentPage == i));
            }
            PaginatorItems = currentPageItems;
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
        public int FoundMaterialsCount
        {
            get => _foundMaterialsCount;
            set => SetProperty(ref _foundMaterialsCount, value);
        }

        public RelayCommand GoToPreviousPageCommand
        {
            get
            {
                if (_goToPreviousPageCommand == null)
                {
                    _goToPreviousPageCommand = new RelayCommand(PerformGoToPreviousPage);
                }
                return _goToPreviousPageCommand;
            }

            set => SetProperty(ref _goToPreviousPageCommand, value);
        }

        private void PerformGoToNextPage(object obj)
        {
            CurrentPage++;
            LoadMaterials();
        }

        private RelayCommand _goToPreviousPageCommand;

        public RelayCommand GoToNextPageCommand
        {
            get
            {
                if (_goToNextPageCommand == null)
                {
                    _goToNextPageCommand = new RelayCommand(PerformGoToNextPage);
                }
                return _goToNextPageCommand;
            }

            set => SetProperty(ref _goToNextPageCommand, value);
        }

        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }
        public IList<PaginatorItem> PaginatorItems
        {
            get => _paginatorItems;
            set => SetProperty(ref _paginatorItems, value);
        }
        public int AllMaterialsCount
        {
            get => _allMaterialsCount;
            set => SetProperty(ref _allMaterialsCount, value);
        }
        public RelayCommand GoToSelectedPageCommand
        {
            get
            {
                if (_goToSelectedPageCommand == null)
                {
                    _goToSelectedPageCommand = new RelayCommand(PerformPaginationCommand);
                }
                return _goToSelectedPageCommand;
            }

            set => _goToSelectedPageCommand = value;
        }

        public RelayCommand EditMaterialCommand
        {
            get
            {
                if (_editMaterialCommand == null)
                {
                    _editMaterialCommand = new RelayCommand(PerformEditCommand);
                }
                return _editMaterialCommand;
            }
            set => _editMaterialCommand = value;
        }

        public RelayCommand AddNewMaterialCommand
        {
            get
            {
                if (_addNewMaterialCommand == null)
                {
                    _addNewMaterialCommand = new RelayCommand(PerformAddMaterial);
                }
                return _addNewMaterialCommand;
            }

            set => _addNewMaterialCommand = value;
        }

        private void PerformAddMaterial(object obj)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                           .Navigate<AddEditMaterialViewModel>();
        }

        private void PerformEditCommand(object obj)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .NavigateWithParameter<AddEditMaterialViewModel>(obj as Material);
        }

        private void PerformPaginationCommand(object obj)
        {
            int pageNumber = (int)obj;
            CurrentPage = pageNumber;
            LoadMaterials();
        }

        private void PerformGoToPreviousPage(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadMaterials();
            }
        }

        private RelayCommand _goToNextPageCommand;


        private IEnumerable<Material> _materials;

        private int _currentPage = 1;
        private int _allMaterialsCount;
        private int _foundMaterialsCount;

        private RelayCommand _goToSelectedPageCommand;

        private RelayCommand _editMaterialCommand;

        private RelayCommand _addNewMaterialCommand;
    }
}
