namespace DraftDesktopApp.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel => NavigationService.CurrentNavigator;

        public NavigationViewModel()
        {
            NavigationService.Navigated += OnNavigated;
        }

        private void OnNavigated()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
