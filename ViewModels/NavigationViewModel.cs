using DraftDesktopApp.Services;

namespace DraftDesktopApp.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel =>
            DependencyService.Get<INavigationService<ViewModelBase>>().CurrentNavigator;

        public NavigationViewModel()
        {
            DependencyService.Get<INavigationService<ViewModelBase>>().Navigated += 
                OnNavigated;
        }

        private void OnNavigated()
        {
            System.Diagnostics.Debug.WriteLine("navigated");
        }
    }
}
