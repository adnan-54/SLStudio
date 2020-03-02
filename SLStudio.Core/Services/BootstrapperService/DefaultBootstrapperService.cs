using SLStudio.Core.Properties;
using SLStudio.Core.Utilities.DependenciesContainer;
using SLStudio.Core.Utilities.ModuleBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio.Core.Services.BootstrapperService
{
    internal class DefaultBootstrapperService : IBootstrapperService
    {
        private readonly IContainer container;
        private readonly ISplashScreen splashScreen;
        private readonly ILogger logger;
        private List<IModule> modules;
        private bool isInitialized;

        public DefaultBootstrapperService(Container container, ISplashScreen splashScreen, ILoggingFactory loggingFactory)
        {
            this.container = container;
            this.splashScreen = splashScreen;
            logger = loggingFactory.GetLoggerFor<DefaultBootstrapperService>();
            modules = new List<IModule>();
            isInitialized = false;
        }

        public IEnumerable<IModule> GetModules()
        {
            foreach (var module in modules)
                yield return module;
        }

        public async Task Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;

            await Task.Run(async () =>
            {
                await Task.Delay(500);
                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "SLStudio.*.dll");
                foreach (var file in files)
                {
                    var assemblyName = Path.GetFileName(file);
                    splashScreen.CurrentModule = assemblyName;
                    logger.Debug($"Loading assembly {assemblyName}");
                    Assembly.LoadFrom(file);
                    await Task.Delay(100);
                }

                var types = AppDomain.CurrentDomain.GetAssemblies()
                      .SelectMany(assembly => assembly.GetTypes())
                      .Where(type => type.IsClass && type.Name == "Module" && type.IsSubclassOf(typeof(ModuleBase)));

                foreach (var type in types)
                    modules.Add(Activator.CreateInstance(type) as IModule);

                modules = modules.OrderByDescending(p => p.ModulePriority).ToList();

                await Task.Delay(500);
                foreach (var module in modules)
                {
                    splashScreen.CurrentModule = module.ModuleName;
                    logger.Debug($"Loading module {module.ModuleName}");

                    if (module.ShouldBeLoaded)
                        module.Register(container);

                    if (!Settings.Default.FastSplashScreen)
                        await Task.Delay(Settings.Default.SplashScreenSleepTime);

                    await Task.Delay(100);
                }
                splashScreen.CurrentModule = null;
                await Task.Delay(500);
            });
        }
    }
}