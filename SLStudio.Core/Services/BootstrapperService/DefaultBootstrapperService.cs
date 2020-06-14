using SLStudio.Core.Properties;
using SLStudio.Core.Utilities.DependenciesContainer;
using SLStudio.Core.Utilities.ModuleBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
            logger = loggingFactory.GetLogger<DefaultBootstrapperService>();
            modules = new List<IModule>();
            isInitialized = false;
        }

        public IEnumerable<IModule> Modules => modules;

        public async Task Initialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;

            await Task.Run(() =>
            {
                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "SLStudio.*.dll");
                foreach (var file in files)
                {
                    var assemblyName = Path.GetFileName(file);
                    logger.Debug($"Loading assembly {assemblyName}");
                    splashScreen.CurrentModule = assemblyName;
                    Assembly.LoadFrom(file);
                }

                var types = AppDomain.CurrentDomain.GetAssemblies()
                      .SelectMany(assembly => assembly.GetTypes())
                      .Where(type => type.IsClass && type.Name == "Module" && type.IsSubclassOf(typeof(ModuleBase)));

                foreach (var type in types)
                    modules.Add(Activator.CreateInstance(type) as IModule);

                modules = modules.OrderByDescending(p => p.ModulePriority).ToList();

                foreach (var module in modules)
                {
                    splashScreen.CurrentModule = module.ModuleName;
                    logger.Debug($"Loading module {module.ModuleName}");

                    foreach (var uri in module.GetResources())
                        if (uri != null)
                            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });

                    if (module.ShouldRegister)
                        module.Register(container);
                }
                splashScreen.CurrentModule = null;
            });
        }
    }
}