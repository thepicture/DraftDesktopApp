using DraftDesktopApp.Services;
using DraftDesktopApp.ViewModels;
using System.Windows;

namespace DraftDesktopApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyService.Register<ViewModelNavigationService>();

            Window window = new NavigationView
            {
                DataContext = new NavigationViewModel()
            };
            window.Show();

            DependencyService
                .Get<INavigationService<ViewModelBase>>()
                .Navigate<MaterialViewModel>();
        }
    }
}
