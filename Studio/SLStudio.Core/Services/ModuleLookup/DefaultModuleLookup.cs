using SimpleInjector;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultModuleLookup : IModuleLookup
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultModuleLookup>();

        private ISplashScreen splashScreen;
        private Container container;
        private IObjectFactory objectFactory;
        private readonly List<IModule> modules = new List<IModule>();
        private bool alreadyInitialized = false;
        private bool alreadyLoaded = false;

        public void Initialize(ISplashScreen splashScreen, Container container, IObjectFactory objectFactory)
        {
            if (alreadyInitialized)
                return;
            alreadyInitialized = true;

            this.splashScreen = splashScreen;
            this.container = container;
            this.objectFactory = objectFactory;
        }

        public IEnumerable<IModule> Modules => modules;

        public async Task Load()
        {
            await Task.Run(() =>
            {
                if (!alreadyInitialized)
                    throw new InvalidOperationException($"{nameof(DefaultModuleLookup)} is not initialized yet");

                if (alreadyLoaded)
                    return;
                alreadyLoaded = true;

                logger.Debug($"Searching modules");
                var types = FindModules();

                var instances = CreateInstances(types);
                modules.AddRange(instances);

                MergeResources(modules);
                RegisterModules(modules);
            });

            container.Verify();

            await RunModules(modules);
        }

        private IEnumerable<Type> FindModules()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsClass && type.Name == "Module" && type.IsSubclassOf(typeof(StudioModule)));
        }

        private IEnumerable<IModule> CreateInstances(IEnumerable<Type> types)
        {
            var modules = new List<IModule>();
            foreach (var type in types)
            {
                logger.Debug($"Creating instance for {type.FullName} from assembly {type.Assembly}");
                modules.Add(Activator.CreateInstance(type) as IModule);
            }

            return modules.OrderByDescending(m => m.Priority);
        }

        private void MergeResources(IEnumerable<IModule> modules)
        {
            foreach (var module in modules.Where(m => m.ShouldBeLoaded))
                module.MergeResources(splashScreen);
        }

        private void RegisterModules(IEnumerable<IModule> modules)
        {
            foreach (var module in modules.Where(m => m.ShouldBeLoaded))
                module.Register(splashScreen, container);
        }

        private async Task RunModules(IEnumerable<IModule> modules)
        {
            foreach (var module in modules.Where(m => m.ShouldBeLoaded))
                await module.Run(splashScreen, objectFactory);
        }
    }
}