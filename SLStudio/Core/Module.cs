using Caliburn.Micro;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.SplashScreen.ViewModels;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Core.Services.LoggingService;
using SLStudio.Core.Utilities.CommandLinesArguments;
using SLStudio.Core.Utilities.ErrorHandler;
using SLStudio.Core.Utilities.ObjectFactory;
using SLStudio.Core.Utilities.WindowManager;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        private bool isRegistred = false;

        public override ModulePriority ModulePriority => ModulePriority.Core;
        public override string ModuleName => "SLStudio Core";
        public override string ModuleDescrition => "Core module.";
        public override bool ShouldBeLoaded => isRegistred;

        public override void Register(IContainer container)
        {
            if (isRegistred)
                return;

            isRegistred = true;

            //Factories
            container.Singleton<IObjectFactory, DefaultObjectFactory>();
            container.Singleton<ILoggingFactory, DefaultLoggingFactory>();

            //Services
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IBootstrapperService, DefaultBootstrapperService>();
            container.Singleton<IWindowManager, DefaultWindowManager>();
            container.Singleton<ILoggingService, DefaultLoggingService>();

            //Utilities
            container.Singleton<ICommandLineArguments, DefaultCommandLineArguments>();
            container.Singleton<IErrorHandler, DefaultErrorHandler>();

            //Modules
            container.Instance(container);
            container.Singleton<ISplashScreen, SplashScreenViewModel>();
            container.Singleton<IShell, ShellViewModel>();
        }
    }
}