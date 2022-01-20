using System;

namespace DraftDesktopApp.Services
{
    /// <summary>
    /// Определяет методы для навигации посредством 
    /// элементов навигации.
    /// </summary>
    /// <typeparam name="TNavigator">Тип элемента навигации.</typeparam>
    public interface INavigationService<TNavigator>
    {
        /// <summary>
        /// Вызывается, когда произошла навигация.
        /// </summary>
        event Action Navigated;

        /// <summary>
        /// Возвращает текущий навигатор.
        /// </summary>
        TNavigator CurrentNavigator { get; }

        /// <summary>
        /// Поместить в стек новый элемент навигации.
        /// </summary>
        /// <typeparam name="T">Элемент навигации.</typeparam>
        void Navigate<T>() where T : TNavigator;

        /// <summary>
        /// Поместить в стек новый элемент навигации с передачей объекта.
        /// </summary>
        /// <typeparam name="T">Элемент навигации.</typeparam>
        /// <param name="param">Переданный объект.</param>
        void NavigateWithParameter<T>(object param) where T : TNavigator;

        /// <summary>
        /// Вернуться на предыдущий элемент навигации.
        /// </summary>
        void GoBack();
    }
}
