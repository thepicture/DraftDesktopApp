using DraftDesktopApp.ViewModels;
using System;
using System.Collections.Generic;

namespace DraftDesktopApp.Services
{
    /// <summary>
    /// Реализует методы для навигации через модели представления.
    /// </summary>
    public class ViewModelNavigationService : INavigationService<ViewModelBase>
    {
        public ViewModelBase CurrentNavigator { get; private set; }
        public Stack<ViewModelBase> Journal;
        public event Action Navigated;

        public ViewModelNavigationService()
        {
            Journal = new Stack<ViewModelBase>();
        }

        public void Navigate<T>() where T : ViewModelBase
        {
            Journal.Push((T)Activator.CreateInstance(typeof(T)));
            CurrentNavigator = Journal.Peek();
            Navigated?.Invoke();
        }

        public void NavigateWithParameter<T>(object param) where T : ViewModelBase
        {
            Journal.Push((T)Activator.CreateInstance(typeof(T), new object[] { param }));
            CurrentNavigator = Journal.Peek();
            Navigated?.Invoke();
        }

        public void GoBack()
        {
            if (Journal.Count != 0)
            {
                ViewModelBase viewModel = Journal.Pop();
                CurrentNavigator = viewModel;
                Navigated?.Invoke();
            }
        }
    }
}
