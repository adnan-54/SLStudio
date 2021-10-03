namespace SLStudio.Core
{
    internal class CoreModule : Module
    {
        protected override void Register(IModuleRegister register)
        {
            //MainMenu
            register.ViewModel<IMainMenu, MainMenuViewModel>();
            register.View<MainMenuView, IMainMenu>();

            //Shell
            register.ViewModel<IShell, ShellViewModel>();
            register.Window<ShellView, IShell>();
            register.Singleton<ShellOpeningStrategy>();

            //StatusBar
            register.ViewModel<IStatusBar, StatusBarViewModel>();
            register.View<StatusBarView, IStatusBar>();
        }
    }
}