using System;
using System.Windows;
using System.Windows.Threading;
using SplashScreen = SLStudio.Core.Modules.SplashScreen.Views.SplashScreen;

namespace SLStudio.Core
{
    class SplashScreenService : ISplashScreenService
    {
        private SplashScreen splashScreen;
        private bool canShow;
        private bool canClose;

        public SplashScreenService()
        {
            canShow = true;
            canClose = false;
        }
        
        public void Show()
        {
            if (canShow)
            {
                canShow = false;
                canClose = true;

                splashScreen = new SplashScreen();
                splashScreen.Show();
            }
        }

        public void Hide()
        {
            if (splashScreen != null)
                splashScreen.Hide();
        }

        public void Close()
        {
            if(splashScreen != null && canClose)
            { 
                canClose = false;
                splashScreen.Close();
            }
        }

        public void UpdateStatus(string status)
        {
            if (splashScreen != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => splashScreen.UpdateStatus(status)));
            }
        }
    }

    public interface ISplashScreenService
    {
        void Show();
        void Hide();
        void Close();
        void UpdateStatus(string status);
    }
}
