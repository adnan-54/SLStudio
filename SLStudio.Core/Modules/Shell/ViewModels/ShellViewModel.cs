namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : ViewModel, IShell
    {
        public ShellViewModel(IMainMenu mainMenu)
        {
            MainMenu = mainMenu;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu
        {
            get => GetProperty(() => MainMenu);
            set => SetProperty(() => MainMenu, value);
        }
    }
}