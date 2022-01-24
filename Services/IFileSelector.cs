namespace DraftDesktopApp.Services
{
    /// <summary>
    /// Определяет метод для выбора файла с возвращаемым типом, 
    /// зависящим от реализации.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого файла.</typeparam>
    public interface IFileSelector<T>
    {
        /// <summary>
        /// Метод для попытки выбора файла.
        /// </summary>
        /// <param name="value">Данные файла, если файл выбран.</param>
        /// <param name="pattern">Паттерн для выбора файла. 
        /// По умолчанию является пустой строкой.</param>
        /// <returns>Истина, если файл выбран. Иначе ложь.</returns>
        bool TryToSelect(out T value, string pattern = "");
    }
}
