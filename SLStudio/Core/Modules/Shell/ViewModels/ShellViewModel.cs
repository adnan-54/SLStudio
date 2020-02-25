using Caliburn.Micro;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Conductor<object>, IShell
    {
        private IStatusBar statusBar;
        private IToolBar toolBar;
        private IMainMenu mainMenu;

        public ShellViewModel(IMainMenu mainMenu, IToolBar toolBar, IStatusBar statusBar)
        {
            MainMenu = mainMenu;
            ToolBar = toolBar;
            StatusBar = statusBar;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu
        {
            get => mainMenu;
            set
            {
                mainMenu = value;
                NotifyOfPropertyChange(() => MainMenu);
            }
        }

        public IToolBar ToolBar
        {
            get => toolBar;
            set
            {
                toolBar = value;
                NotifyOfPropertyChange(() => ToolBar);
            }
        }

        public IStatusBar StatusBar
        {
            get => statusBar;
            set
            {
                statusBar = value;
                NotifyOfPropertyChange(() => StatusBar);
            }
        }
    }
}
