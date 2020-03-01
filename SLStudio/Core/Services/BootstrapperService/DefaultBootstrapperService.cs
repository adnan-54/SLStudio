using SLStudio.Core.Utilities.DependenciesContainer;
using SLStudio.Core.Utilities.ModuleBase;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Services.BootstrapperService
{
    internal class DefaultBootstrapperService : IBootstrapperService
    {
        private readonly IContainer container;
        private readonly ISplashScreen splashScreen;
        private List<IModule> modules;
        private bool isInitialized;

        public DefaultBootstrapperService(Container container, ISplashScreen splashScreen)
        {
            this.container = container;
            this.splashScreen = splashScreen;
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