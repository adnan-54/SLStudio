using Caliburn.Micro;
using SLStudio.Core.Modules.SplashScreen;
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
        private readonly SimpleContainer container;
        private readonly ISplashScreen splashScreen;
        private readonly List<IModule> modules;

        public DefaultBootstrapperService(SimpleContainer container, ISplashScreen splashScreen)
        {
            this.container = container;
            this.splashScreen = splashScreen;
            modules = new List<IModule>();
        }

        public IList<IModule> Modules => modules;

        public async Task Initialize()
        {
            var windowManager = IoC.Get<IWindowManager>();
            await windowManager.ShowWindowAsync(splashScreen);

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

                var orderedModules = modules.OrderByDescending(p => p.ModulePriority);

                await Task.Delay(500);
                foreach (var module in orderedModules)
                {
                    if (module != null && module.ShouldBeLoaded)
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
        }
    }
}