using System.Windows;

namespace SLStudio
{
    public interface ISplashScreen : IStudioService
    {
        string Status { get; }

        void Show();

        void SetView<TView>(TView view)
            where TView : Window, ISplashScreenView;

        void UpdateStatus(string status);

        void UpdateStatusFormat(string format, params string[] args);

        void Close();
    }
}