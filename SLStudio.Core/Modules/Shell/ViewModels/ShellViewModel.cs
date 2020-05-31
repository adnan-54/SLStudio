namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : ViewModel, IShell
    {
        public ShellViewModel(IMainMenu mainMenu, IToolBar toolBar, IStatusBar statusBar, ICommandLineArguments commandLineArguments)
        {
            MainMenu = mainMenu;
            ToolBar = toolBar;
            StatusBar = statusBar;

            DisplayName = "SLStudio";
            if (commandLineArguments.DebugMode)
                DisplayName += " (debug)";
        }

        public IMainMenu MainMenu
        {
            get => GetProperty(() => MainMenu);
            set => SetProperty(() => MainMenu, value);
        }

        public IToolBar ToolBar
        {
            get => GetProperty(() => ToolBar);
            set => SetProperty(() => ToolBar, value);
        }

        public IStatusBar StatusBar
        {
            get => GetProperty(() => StatusBar);
            set => SetProperty(() => StatusBar, value);
        }
    }
}