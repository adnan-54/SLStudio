using Caliburn.Micro;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class BootstrapperService : IBootstrapperService
    {
        private readonly SimpleContainer container;
        private readonly List<IModule> modules;

        public BootstrapperService(SimpleContainer container)
        {
            this.container = container;

            modules = new List<IModule>();
        }

        public IList<IModule> Modules => modules;

        public async Task Initialize()
        {
            await LoadModulesAsync();
        }

        private async Task LoadModulesAsync()
        {
            await Task.Run(async () =>
            {
                GetType().Assembly.GetTypes().Where(type => type.IsClass && type.Name.Equals("Module") && type.GetInterface(nameof(IModule)) != null)
                .ToList()
                .ForEach(type =>
                {
                    var instance = Activator.CreateInstance(type) as IModule;
                    modules.Add(instance);
                });

                var orderedModules = modules.OrderByDescending(p => p.ModulePriority).ToList();
                modules.Clear();
                modules.AddRange(orderedModules);
                orderedModules.Clear();

                foreach (var module in modules)
                {
                    if (module != null && module.ShouldBeLoaded)
                    {
                        if (!Settings.Default.FastSplashScreen)
                            await Task.Delay(Settings.Default.SplashScreenSleepTime);

                        module.Register(container);
                    }
                }
            });
        }
    }

    public interface IBootstrapperService
    {
        Task Initialize();
    }
}