using Caliburn.Micro;
using SLStudio.Core.Modules.MainMenu.ViewModels;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.SplashScreen.ViewModels;
using SLStudio.Core.Modules.StatusBar.ViewModels;
using SLStudio.Core.Modules.ToolBar.ViewModels;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Core.Services.LoggingService;
using SLStudio.Core.Services.SettingsService;
using SLStudio.Core.Utilities.CommandLinesArguments;
using SLStudio.Core.Utilities.ErrorHandler;
using SLStudio.Core.Utilities.ObjectFactory;
using SLStudio.Core.Utilities.ThemeManager;
using SLStudio.Core.Utilities.WindowManager;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        private static bool isRegistred = false;

        public override ModulePriority ModulePriority => ModulePriority.Core;
        public override string ModuleName => "SLStudio Core";
        public override string ModuleDescrition => "Core module.";
        public override bool ShouldRegister => !isRegistred;

        public override void Register(IContainer container)
        {
            if (isRegistred)
                return;
            isRegistred = true;

            container.Instance(container);
            container.Singleton<IObjectFactory, DefaultObjectFactory>();
            container.Singleton<ILoggingFactory, DefaultLoggingFactory>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IBootstrapperService, DefaultBootstrapperService>();
            container.Singleton<ILoggingService, DefaultLoggingService>();
            container.Singleton<ICommandLineArguments, DefaultCommandLineArguments>();
            container.Singleton<IErrorHandler, DefaultErrorHandler>();
            container.Singleton<IWindowManager, DefaultWindowManager>();
            container.Singleton<ISettingsService, DefaultSettingsService>();
            container.Singleton<IThemeManager, DefaultThemeManager>();

            container.Singleton<ISplashScreen, SplashScreenViewModel>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<IMainMenu, MainMenuViewModel>();
            container.Singleton<IToolBar, ToolBarViewModel>();
            container.Singleton<IStatusBar, StatusBarViewModel>();
        }
    }
}