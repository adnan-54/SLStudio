using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio
{
    internal class ModuleLoader : Service, IModuleLoader
    {
        private static readonly ILogger logger = LogManager.GetLogger<ModuleLoader>();

        private readonly IModuleRegister moduleRegister;
        private readonly IModuleScheduler moduleScheduler;
        private readonly IObjectFactory objectFactory;
        private readonly List<IModule> loadedModules;

        public ModuleLoader(IModuleRegister moduleRegister, IModuleScheduler moduleScheduler, IObjectFactory objectFactory)
        {
            this.moduleRegister = moduleRegister;
            this.moduleScheduler = moduleScheduler;
            this.objectFactory = objectFactory;
            loadedModules = new List<IModule>();
        }

        public IEnumerable<IModule> LoadedModules => loadedModules;

        public async Task LoadModules(IEnumerable<IModule> modules)
        {
            if (loadedModules.Any())
                throw new InvalidOperationException("Modules already loaded");

            logger.Debug("Starting to load modules...");

            var targetModules = modules.Where(m => m.CanBeLoaded)
                                       .OrderByDescending(m => m.Priority);

            RegisterModules(targetModules);
            ScheduleModules(targetModules);
            await RunModules(targetModules);
            loadedModules.AddRange(targetModules);

            logger.Debug("Finished to load modules");
        }

        private void RegisterModules(IEnumerable<IModule> modules)
        {
            logger.Debug("Starting to register modules...");

            foreach (var module in modules)
                module.OnRegister(moduleRegister);

            logger.Debug("Finished to register modules");
        }

        private void ScheduleModules(IEnumerable<IModule> modules)
        {
            logger.Debug("Starting to schedule modules...");

            foreach (var module in modules)
                module.OnSchedule(moduleScheduler);

            logger.Debug("Finished to schedule modules");
        }

        private async Task RunModules(IEnumerable<IModule> modules)
        {
            logger.Debug("Starting to run modules...");

            var tasks = modules.Select(m => m.OnRun(objectFactory));
            await Task.WhenAll(tasks);

            logger.Debug("Finished running modules");
        }
    }
}