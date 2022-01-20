using System;
using System.Collections.Generic;
using System.Linq;

namespace DraftDesktopApp.Services
{
    /// <summary>
    /// Предоставляет доступ к реализациям на основе интерфейсов.
    /// </summary>
    public static class DependencyService
    {
        private static readonly Dictionary<Type, object> _implementations =
            new Dictionary<Type, object>();

        /// <summary>
        /// Получает реализацию на основе интерфейса.
        /// </summary>
        /// <typeparam name="T">Интерфейс, имеющий реализацию.</typeparam>
        /// <returns>Реализация.</returns>
        public static T Get<T>() => (T)_implementations
                .First(i => typeof(T).IsAssignableFrom(i.Key))
                .Value;

        /// <summary>
        /// Регистрирует реализацию.
        /// </summary>
        /// <typeparam name="T">Тип реализации.</typeparam>
        public static void Register<T>()
        {
            _implementations.Add(typeof(T), Activator.CreateInstance(typeof(T)));
        }
    }
}
