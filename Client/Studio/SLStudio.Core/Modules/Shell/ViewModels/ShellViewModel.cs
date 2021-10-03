namespace SLStudio.Core
{
    internal class ShellViewModel : WindowViewModel, IShell
    {
        public ShellViewModel(IMainMenu mainMenu, IStatusBar statusBar)
        {
            MainMenu = mainMenu;
            StatusBar = statusBar;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu { get; }

        public IStatusBar StatusBar { get; }
    }

    public interface IShell : IWindowViewModel
    {
    }
}