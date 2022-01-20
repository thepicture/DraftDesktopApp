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

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
