namespace SLStudio.Core
{
    internal class CoreModule : Module
    {
        protected override void Register(IModuleRegister register)
        {
            //MenuConfiguration
            register.MenuConfiguration<CoreMenuConfiguration>();


            //Shell
            register.ViewModel<IShell, ShellViewModel>(LifeStyle.Singleton);
            register.Window<ShellView, IShell>();
            register.Singleton<ShellOpeningStrategy>();

            //StartPage
            register.ViewModel<IStartPage, StartPageViewModel>(LifeStyle.Singleton);
            register.View<StartPageView, IStartPage>();


            //todo: move this to framework
            //MainMenu
            register.ViewModel<IMainMenu, MainMenuViewModel>(LifeStyle.Singleton);
            register.View<MainMenuView, IMainMenu>();

            //StatusBar
            register.ViewModel<IStatusBar, StatusBarViewModel>(LifeStyle.Singleton);
            register.View<StatusBarView, IStatusBar>();

            //ToolBar
            register.ViewModel<IToolBar, ToolBarViewModel>(LifeStyle.Singleton);
            register.View<ToolBarView, IToolBar>();
        }
    }
}
