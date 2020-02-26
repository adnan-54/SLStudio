using Caliburn.Micro;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Conductor<object>, IShell
    {
        private IStatusBar statusBar;
        private IToolBar toolBar;
        private IMainMenu mainMenu;

        public ShellViewModel(IMainMenu mainMenu, IToolBar toolBar, IStatusBar statusBar, ICommandLineArguments commandLineArguments)
        {
            MainMenu = mainMenu;
            ToolBar = toolBar;
            StatusBar = statusBar;

            if (commandLineArguments.DebuggingMode)
                DisplayName = "SLStudio - Debugging Mode";
            else
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
