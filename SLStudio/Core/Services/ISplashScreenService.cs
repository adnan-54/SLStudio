using SLStudio.Core.Modules;
using SLStudio.Core.Modules.SplashScreen.Views;

namespace SLStudio.Core
{
    class SplashScreenService : ISplashScreenService
    {
        private SplashScreen splashScreen;

        public SplashScreenService()
        {
            splashScreen = new SplashScreen();
        }

        public void Close()
        {
            splashScreen.Close();
        }

        public void Show()
        {
            splashScreen.Show();
        }

        public void UpdateStatus(string status)
        {
            splashScreen.statusText.Text = status;
        }
    }

    public interface ISplashScreenService
    {
        void Show();
        void Close();
        void UpdateStatus(string status);
    }
}
