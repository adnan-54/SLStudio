using System.Windows.Input;

namespace SLStudio.Core.Modules.SplashScreen.ViewModels
{
    internal class SplashScreenViewModel : ViewModel, ISplashScreen
    {
        public SplashScreenViewModel()
        {
            DisplayName = "SLStudio";
        }

        public bool IsMouseDown
        {
            get => GetProperty(() => IsMouseDown);
            set => SetProperty(() => IsMouseDown, value);
        }

        public string CurrentModule
        {
            get => GetProperty(() => CurrentModule);
            set => SetProperty(() => CurrentModule, value);
        }

        public string Version => GetType().Assembly.GetName().Version.ToString();

        public void OnMouseDown(MouseButtonEventArgs args)
        {
            if (args.ChangedButton == MouseButton.Left)
                IsMouseDown = true;
        }

        public void OnMouseUp() => IsMouseDown = false;
    }
}