using Caliburn.Micro;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    internal class MainWindowViewModel : Screen, IMainWindow
    {
        private readonly IMainMenu mainMenu;
        private readonly IToolbar toolbar;
        private readonly IShell shell;
        private readonly IStatusBar statusBar;

        public MainWindowViewModel(IMainMenu mainMenu, IToolbar toolbar, IShell shell, IStatusBar statusBar)
        {
            this.mainMenu = mainMenu;
            this.toolbar = toolbar;
            this.shell = shell;
            this.statusBar = statusBar;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu => mainMenu;

        public IToolbar Toolbar => toolbar;

        public IShell Shell => shell;

        public IStatusBar StatusBar => statusBar;
    }
}