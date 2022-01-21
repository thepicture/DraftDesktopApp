using DraftDesktopApp.Commands;
using DraftDesktopApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DraftDesktopApp.ViewModels
{
    /// <summary>
    /// Базовый тип модели представления.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        private string _title;
        private bool _isBusy;
        private RelayCommand _goBackCommand;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public RelayCommand GoBackCommand
        {
            get
            {
                if (_goBackCommand != null)
                {
                    _goBackCommand = new RelayCommand(PerformGoBack);
                }
                return _goBackCommand;
            }

            set => _goBackCommand = value;
        }

        private void PerformGoBack(object obj)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .GoBack();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

    }
}
