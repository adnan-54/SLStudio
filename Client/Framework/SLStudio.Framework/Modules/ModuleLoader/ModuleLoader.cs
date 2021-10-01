using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio
{
    internal class ModuleLoader : StudioService, IModuleLoader
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<ModuleLoader>();

        private readonly ISplashScreen splashScreen;
        private readonly IModuleContainerFactory moduleContainerFactory;
        private readonly IObjectFactory objectFactory;

        public ModuleLoader(ISplashScreen splashScreen, IModuleContainerFactory moduleContainerFactory, IObjectFactory objectFactory)
        {
            this.splashScreen = splashScreen;
            this.moduleContainerFactory = moduleContainerFactory;
            this.objectFactory = objectFactory;
        }

        public IEnumerable<IModuleInfo> LoadedModules { get; private set; }

        public async Task LoadModules()
        {
            if (LoadedModules != null)
                return;

            logger.Debug("Loading modules...");
            splashScreen.UpdateStatus("Loading modules...");

            var modules = FindModules();

            var containers = modules.Where(module => module.ShouldBeLoaded).OrderByDescending(module => module.Priority).Select(moduleContainerFactory.CreateContainerFor).ToList();

            foreach (var container in containers)
                LoadModule(container);

            var actions = containers.Select(c => RunModule(c));
            await Task.WhenAll(actions);

            LoadedModules = containers.Select(container => new ModuleInfo(container));

            var modulesCount = LoadedModules.Count();
            logger.Debug($"{modulesCount} modules loaded...");
            splashScreen.UpdateStatus("{0} modules loaded...", modulesCount);
        }

        private IEnumerable<IStudioModule> FindModules()
        {
            logger.Debug("Searching modules...");
            splashScreen.UpdateStatus("Searching modules...");

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => !t.IsAbstract && t.IsClass && t.IsAssignableTo(typeof(StudioModule))).ToList();

            logger.Debug($"{types.Count} modules found.");
            splashScreen.UpdateStatus("{0} modules found.", types.Count);

            return types.Select(CreateModule);
        }

        private IStudioModule CreateModule(Type type)
        {
            var name = type.Assembly.GetName().Name;
            logger.Debug($"Creating instance for {name}...");
            splashScreen.UpdateStatus("Creating instance for {0}...", name);

            return (IStudioModule)Activator.CreateInstance(type);
        }

        private void LoadModule(IModuleContainer container)
        {
            var module = container.Module;

            try
            {
                logger.Debug($"Loading module {module.Name}");
                splashScreen.UpdateStatus("Loading module {0}", module.Name);

                module.Load(container);

                logger.Debug($"Module {module.Name} loaded successfully");
                splashScreen.UpdateStatus("Module {0} loaded successfully", module.Name);
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to load module {module.Name}");
                logger.Fatal(ex);

                throw;
            }
        }

        private Task RunModule(IModuleContainer container)
        {
            var module = container.Module;
            var scheduler = container.Scheduler;
            try
            {
                logger.Debug($"Running module {module.Name}");
                splashScreen.UpdateStatus("Running module {0}", module.Name);

                foreach (var action in scheduler.ScheduledActions)
                    action.Run(objectFactory);

                var tasks = scheduler.ScheduledTasks.Select(task => task.Run(objectFactory));
                return Task.WhenAll(tasks).ContinueWith(t =>
                {
                    if (t.Exception != null)
                        throw t.Exception;

                    logger.Debug($"Module {module.Name} ran successfully");
                    splashScreen.UpdateStatus("Module {0} ran successfully", module.Name);
                });
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to run module {module.Name}");
                logger.Fatal(ex);

                throw;
            }
        }
    }
}