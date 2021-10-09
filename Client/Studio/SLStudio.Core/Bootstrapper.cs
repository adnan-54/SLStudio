using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core
{
    public class Bootstrapper
    {
        private readonly ISplashScreenManager splashScreenManager;
        private readonly IAssemblyFinder assemblyFinder;
        private readonly IAssemblyLoader assemblyLoader;
        private readonly IModuleFinder moduleFinder;
        private readonly IModuleLoader moduleLoader;
        private readonly IWindowManager windowManager;
        private readonly IContainer container;

        public Bootstrapper(IServiceContainer serviceContainer, ISplashScreenManager splashScreenManager)
        {
            this.splashScreenManager = splashScreenManager;

            assemblyFinder = serviceContainer.GetService<IAssemblyFinder>();
            assemblyLoader = serviceContainer.GetService<IAssemblyLoader>();
            moduleFinder = serviceContainer.GetService<IModuleFinder>();
            moduleLoader = serviceContainer.GetService<IModuleLoader>();
            windowManager = serviceContainer.GetService<IWindowManager>();
            container = serviceContainer.GetService<IContainer>();
        }

        private async Task Run()
        {
            var assemblies = assemblyFinder.FindAssemblies();
            assemblyLoader.LoadAssemblies(assemblies);
            var modules = moduleFinder.FindModules();
            await moduleLoader.LoadModules(modules);
            container.Verify();
            windowManager.Show<IShell>();
            splashScreenManager.Close();
        }

        public static Task Run(Application application, ISplashScreen splashScreen)
        {
            var container = FrameworkModule.GetServiceContainer(application);
            var splashScreenManager = container.GetService<ISplashScreenManager>();
            splashScreenManager.SetSplashScreen(splashScreen);
            var bootstrapper = new Bootstrapper(container, splashScreenManager);
            return Task.Run(bootstrapper.Run);
        }
    }
}