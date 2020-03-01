namespace SLStudio.Core.Modules.SplashScreen.ViewModels
{
    internal class SplashScreenViewModel : ViewModel, ISplashScreen
    {
        public SplashScreenViewModel()
        {
            DisplayName = "SLStudio";
        }

        public string CurrentModule
        {
            get => GetProperty(() => CurrentModule);
            set => SetProperty(() => CurrentModule, value);
        }

        public string Version => GetType().Assembly.GetName().Version.ToString();
    }
}