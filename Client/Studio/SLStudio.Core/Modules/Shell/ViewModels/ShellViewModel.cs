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

            Loaded += ShellViewModel_Loaded;

            Title = "SLStudio";
        }

        private void ShellViewModel_Loaded(object sender, System.EventArgs e)
        {
            Workspace.Show<IStartPage>();
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
