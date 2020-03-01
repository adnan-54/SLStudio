namespace SLStudio.Core.Modules.SplashScreen.ViewModels
{
    internal class SplashScreenViewModel : ViewModel, ISplashScreen
    {
        public SplashScreenViewModel()
        {
            DisplayName = "Teste";
        }

        public string CurrentModule
        {
            get => GetProperty(() => CurrentModule);
            set => SetProperty(() => CurrentModule, value);
        }

        public void Close()
        {
            TryCloseAsync().FireAndForget();
        }
    }
}