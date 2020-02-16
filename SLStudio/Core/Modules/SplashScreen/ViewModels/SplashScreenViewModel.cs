using Caliburn.Micro;

namespace SLStudio.Core.Modules.SplashScreen.ViewModels
{
    internal class SplashScreenViewModel : Screen, ISplashScreen
    {
        public SplashScreenViewModel()
        {
            DisplayName = "SLStudio";
        }

        private string status = Resources.SplashScreen.Loading;

        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public void Close() => TryCloseAsync();
    }
}