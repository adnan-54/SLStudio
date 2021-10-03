using System.Windows;

namespace SLStudio
{
    public interface ISplashScreen : IStudioService
    {
        string Status { get; }

        void Show();

        void SetView(ISplashScreenView view);

        void UpdateStatus(string status);

        void UpdateStatus(string format, params object[] args);

        void Close();
    }
}