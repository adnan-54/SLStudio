using Caliburn.Micro;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    internal class MainWindowViewModel : Screen, IMainWindow
    {
        public MainWindowViewModel(IMainMenu mainMenu, IToolbar toolbar, IShell shell, IStatusBar statusBar)
        {
            MainMenu = mainMenu;
            Toolbar = toolbar;
            Shell = shell;
            StatusBar = statusBar;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu { get; }

        public IToolbar Toolbar { get; }

        public IShell Shell { get; }

        public IStatusBar StatusBar { get; }
    }
}