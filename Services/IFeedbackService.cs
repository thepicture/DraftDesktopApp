namespace DraftDesktopApp.Services
{
    /// <summary>
    /// Определяет методы для обратной связи.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Показывает информацию.
        /// </summary>
        /// <param name="message">Информационное сообщение.</param>
        void ShowInfo(string message);
        /// <summary>
        /// Показывает предупреждение.
        /// </summary>
        /// <param name="message">Сообщение с предупреждением.</param>
        void ShowWarning(string message);
        /// <summary>
        /// Уведомляет об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку 
        /// и подсказки для её исправления.</param>
        void ShowError(string message);
        /// <summary>
        /// Запрашивает подтверждение пользователя.
        /// </summary>
        /// <param name="message">Сообщение с вопросом.</param>
        /// <returns></returns>
        bool AskQuestion(string message);
    }
}
