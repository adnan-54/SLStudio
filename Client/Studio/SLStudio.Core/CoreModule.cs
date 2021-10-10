namespace SLStudio.Core
{
    internal class CoreModule : Module
    {
        protected override void Register(IModuleRegister register)
        {
            //MenuConfiguration
            register.MenuConfiguration<CoreMenuConfiguration>();

            //MainMenu
            register.ViewModel<IMainMenu, MainMenuViewModel>(LifeStyle.Singleton);
            register.View<MainMenuView, IMainMenu>();

            //Shell
            register.ViewModel<IShell, ShellViewModel>(LifeStyle.Singleton);
            register.Window<ShellView, IShell>();
            register.Singleton<ShellOpeningStrategy>();

            //StatusBar
            register.ViewModel<IStatusBar, StatusBarViewModel>(LifeStyle.Singleton);
            register.View<StatusBarView, IStatusBar>();

            //ToolBar
            register.ViewModel<IToolBar, ToolBarViewModel>(LifeStyle.Singleton);
            register.View<ToolBarView, IToolBar>();

            //Workspace
            register.ViewModel<IWorkspace, WorkspaceViewModel>(LifeStyle.Singleton);
            register.View<WorkspaceView, IWorkspace>();
        }
    }
}
