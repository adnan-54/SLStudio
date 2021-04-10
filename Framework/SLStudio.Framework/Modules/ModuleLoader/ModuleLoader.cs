using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLStudio.Logging;

namespace SLStudio
{
    internal class ModuleLoader : StudioService, IModuleLoader
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<ModuleLoader>();

        private readonly IObjectFactory objectFactory;
        private readonly ISplashScreen splashScreen;
        private readonly IModuleRegister moduleRegister;
        private readonly List<IModuleInfo> loadedModules;

        public ModuleLoader(IObjectFactory objectFactory, ISplashScreen splashScreen, IModuleRegister moduleRegister)
        {
            this.objectFactory = objectFactory;
            this.splashScreen = splashScreen;
            this.moduleRegister = moduleRegister;
            loadedModules = new List<IModuleInfo>();
        }

        public IEnumerable<IModuleInfo> LoadedModules => loadedModules;

        public async Task LoadModules(IEnumerable<IStudioModule> modules)
        {
            logger.Debug("Loading modules...");
            splashScreen.UpdateStatus("Loading modules...");

            var registeredModules = RegisterModules(modules.OrderByDescending(module => module.Priority));
        }

        private IEnumerable<IModuleRegister> RegisterModules(IEnumerable<IStudioModule> modules)
        {
            try
            {
                logger.Debug("Registering modules");

                var result = modules.Select(RegisterModule);

                logger.Debug("Modules registered successfully");

                return result;
            }
            catch (Exception ex)
            {
                logger.Warn("Failed to register modules");
                logger.Error(ex);

                throw;
            }
        }

        private IModuleRegister RegisterModule(IStudioModule module)
        {
            splashScreen.UpdateStatusFormat("Registering module {0}...", module.Name);

            module.RegisterModule(moduleRegister);

            return moduleRegister;
        }
    }

    public interface IModuleLoader : IStudioService
    {
        IEnumerable<IModuleInfo> LoadedModules { get; }

        Task LoadModules(IEnumerable<IStudioModule> modules);
    }

    public interface IRegister


    public interface IModuleRegister
    {
    }

    public interface ModuleContainer
    {
    }
}