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

            await Task.Run(async () =>
            {
                GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && type.Name.Equals("Module") && type.GetInterface(nameof(IModule)) != null)
                .ToList()
                .ForEach(type =>
                {
                    var instance = Activator.CreateInstance(type) as IModule;
                    modules.Add(instance);
                });

                modules = modules.OrderByDescending(p => p.ModulePriority).ToList();

                await Task.Delay(500);
                foreach (var module in modules)
                {
                    if (module.ShouldBeLoaded)
                    {
                        if (!Settings.Default.FastSplashScreen)
                            await Task.Delay(Settings.Default.SplashScreenSleepTime);

                        splashScreen.CurrentModule = module.ModuleName;
                        module.Register(container);
                        await Task.Delay(100);
                    }
                }
                splashScreen.CurrentModule = null;
                await Task.Delay(500);
            });

            isInitialized = true;
        }
    }
}