using System.Windows;

namespace SLStudio
{
    public interface ISplashScreen : IStudioService
    {
        string Status { get; }

        void Show();

        void SetView<TView>()
            where TView : Window, ISplashScreenView;

        void UpdateStatus(string status);

        void UpdateStatus(string format, params object[] args);

        void Close();
    }
}