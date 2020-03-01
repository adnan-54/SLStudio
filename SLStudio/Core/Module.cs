using SLStudio.Core.Services.LoggingService;
using SLStudio.Core.Utilities.ErrorHandler;
using SLStudio.Core.Utilities.ObjectFactory;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        public override ModulePriority ModulePriority => ModulePriority.Core;

        public override string ModuleName => "SLStudio Core";

        public override string ModuleDescrition => "Core module.";

        public override void Register(IContainer container)
        {
            //Factories
            container.Singleton<ILoggingFactory, DefaultLoggingFactory>();
            container.Singleton<IObjectFactory, DefaultObjectFactory>();

            //Services
            container.Singleton<ILoggingService, DefaultLoggingService>();

            //Utilities
            container.Singleton<IErrorHandler, DefaultErrorHandler>();

            //Modules
        }
    }
}