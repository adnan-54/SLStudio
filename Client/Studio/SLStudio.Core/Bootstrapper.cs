using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core
{
    public class Bootstrapper
    {
        private readonly IServiceCollection serviceCollection;
        private readonly ISplashScreenManager splashScreenManager;

        public Bootstrapper(IServiceCollection serviceCollection, ISplashScreenManager splashScreenManager)
        {
            this.serviceCollection = serviceCollection;
            this.splashScreenManager = splashScreenManager;
        }

        public Task Run()
        {
            return Task.Run(RunCore);
        }

        private async Task RunCore()
        {
            var assemblyFinder = serviceCollection.Get<IAssemblyFinder>();
            var assemblyLoader = serviceCollection.Get<IAssemblyLoader>();
            var moduleFinder = serviceCollection.Get<IModuleFinder>();
            var moduleLoader = serviceCollection.Get<IModuleLoader>();
            var windowManager = serviceCollection.Get<IWindowManager>();
            var container = serviceCollection.Get<IContainer>();

            var assemblies = assemblyFinder.FindAssemblies();
            assemblyLoader.LoadAssemblies(assemblies);

            var modules = moduleFinder.FindModules();
            await moduleLoader.LoadModules(modules);

            container.Verify();

            windowManager.Show<IShell>();

            splashScreenManager.Close();
        }

        public static Bootstrapper Create(Application application, ISplashScreen splashScreen)
        {
            var container = FrameworkModule.CreateServiceCollection(application);
            var splashScreenManager = container.Get<ISplashScreenManager>();
            splashScreenManager.SetSplashScreen(splashScreen);
            return new Bootstrapper(container, splashScreenManager);
        }
    }
}