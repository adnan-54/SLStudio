using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio
{
    public class Bootstrapper
    {
        private static Bootstrapper instance;

        private readonly IContainer container;
        private readonly Application application;
        private readonly List<IStudioService> services;

        private Bootstrapper(IContainer container, Application application)
        {
            this.container = container;
            this.application = application;
            services = new List<IStudioService>();

            RegisterDefaults();
        }

        public IContainer GetContainer()
        {
            return container;
        }

        public TService GetService<TService>() where TService : class, IStudioService
        {
            return services.OfType<TService>().SingleOrDefault();
        }

        private void RegisterDefaults()
        {
            container.RegisterInstance(container);

            var appInfo = new ApplicationInfo(application);
            var uiSynchronization = new UiSynchronization(appInfo);
            var splashScreen = new SplashScreen(uiSynchronization);
            var assemblyLoader = new AssemblyLoader(splashScreen);
            var messenger = new Messenger();

            var objectFactory = new ObjectFactory(container);
            var moduleContainerFactory = new ModuleContainerFactory(container);
            var moduleLoader = new ModuleLoader(splashScreen, moduleContainerFactory, objectFactory);

            RegisterService<IApplicationInfo>(appInfo);
            RegisterService<IAssemblyLoader>(assemblyLoader);
            RegisterService<ISplashScreen>(splashScreen);
            RegisterService<IMessenger>(messenger);
            RegisterService<IUiSynchronization>(uiSynchronization);

            RegisterService<IObjectFactory>(objectFactory);
            RegisterService<IModuleContainerFactory>(moduleContainerFactory);
            RegisterService<IModuleLoader>(moduleLoader);
        }

        private void RegisterService<TService>(TService service) where TService : class, IStudioService
        {
            if (services.Contains(service))
                return;

            container.RegisterInstance(service);
            services.Add(service);
        }

        public Bootstrapper CreateBootstrapper(Application application)
        {
            if (instance is null)
            {
                var container = CreateContainer();
                IoC.Initialize(container);

                instance = new Bootstrapper(container, application);
            }
            return instance;
        }

        private static IContainer CreateContainer()
        {
            var container = new Container();
            container.Options.ResolveUnregisteredConcreteTypes = true;

            return container;
        }
    }
}