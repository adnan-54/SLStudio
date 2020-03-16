namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : ViewModel, IShell
    {
        public ShellViewModel(IMainMenu mainMenu, IStatusBar statusBar)
        {
            MainMenu = mainMenu;
            StatusBar = statusBar;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu
        {
            get => GetProperty(() => MainMenu);
            set => SetProperty(() => MainMenu, value);
        }

        public IStatusBar StatusBar
        {
            get => GetProperty(() => StatusBar);
            set => SetProperty(() => StatusBar, value);
        }
    }
}