using Caliburn.Micro;
using System.Windows;

namespace SLStudio.Core.Modules.SplashScreen.ViewModels
{
    internal class SplashScreenViewModel : Screen, ISplashScreen
    {
        private string currentModule;

        public SplashScreenViewModel()
        {
            DisplayName = "SLStudio";
        }

        public string CurrentModule
        {
            get => currentModule;
            set
            {
                currentModule = value;
                NotifyOfPropertyChange(() => CurrentModule);
            }
        }

        public string Version => GetType().Assembly.GetName().Version.ToString();

        public async void Close()
        {
            await TryCloseAsync();
        }

        public void CloseButton()
        {
            Application.Current.Shutdown();
        }
    }
}
