namespace SLStudio.Core
{
    internal class ShellViewModel : WindowViewModel, IShell
    {
        public ShellViewModel(IMainMenu mainMenu, IToolBar toolBar, IWorkspace workspace, IStatusBar statusBar)
        {
            MainMenu = mainMenu;
            ToolBar = toolBar;
            Workspace = workspace;
            StatusBar = statusBar;

            DisplayName = "SLStudio";
        }

        public IMainMenu MainMenu { get; }

        public IToolBar ToolBar { get; }

        public IWorkspace Workspace { get; }

        public IStatusBar StatusBar { get; }
    }

    public interface IShell : IWindowViewModel
    {
    }
}