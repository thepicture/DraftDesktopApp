using System.Windows;

namespace DraftDesktopApp.Services
{
    public class MessageBoxFeedbackService : IFeedbackService
    {
        public bool AskQuestion(string message)
        {
            return MessageBox
                .Show(message,
                      "Информация",
                      MessageBoxButton.YesNo,
                      MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowError(string message)
        {
            _ = MessageBox
                .Show(message,
                      "Ошибка",
                      MessageBoxButton.OK,
                      MessageBoxImage.Error);
        }

        public void ShowInfo(string message)
        {
            _ = MessageBox
                .Show(message,
                      "Информация",
                      MessageBoxButton.OK,
                      MessageBoxImage.Information);
        }

        public void ShowWarning(string message)
        {
            _ = MessageBox
                .Show(message,
                      "Предупреждение",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning);
        }
    }
}
