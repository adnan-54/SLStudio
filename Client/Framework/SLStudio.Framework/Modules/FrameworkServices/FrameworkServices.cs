using DevExpress.Mvvm;
using System.Windows;

namespace SLStudio
{
    internal class FrameworkServices : ServiceCollection
    {
        private readonly Application application;

        public FrameworkServices(Application application)
        {
            this.application = application;
        }

        protected override void RegisterServices()
        {
            var applicationInfo = new ApplicationInfo(application) as IApplicationInfo;
            var uiSynchronization = new UiSynchronization(applicationInfo) as IUiSynchronization;
            var splashScreenManager = new SplashScreenManager(uiSynchronization) as ISplashScreenManager;
            var assemblyFinder = new AssemblyFinder() as IAssemblyFinder;
            var assemblyLoader = new AssemblyLoader() as IAssemblyLoader;
            var moduleFinder = new ModuleFinder() as IModuleFinder;
            var container = new Container() as IContainer;
            var messenger = Messenger.Default;
            var moduleRegister = new ModuleRegister(container, messenger) as IModuleRegister;
            var objectFactory = new ObjectFactory(container) as IObjectFactory;
            var menuItemFactory = new MenuItemFactory(objectFactory) as IMenuItemFactory;
            var menuBuilder = new MenuBuilder(objectFactory, menuItemFactory, messenger) as IMenuBuilder;
            var moduleScheduler = new ModuleScheduler() as IModuleScheduler;
            var moduleLoader = new ModuleLoader(moduleRegister, moduleScheduler, objectFactory) as IModuleLoader;
            var viewModelLocator = new ViewModelLocator(messenger) as IViewModelLocator;
            var viewLocator = new ViewLocator(viewModelLocator, messenger) as IViewLocator;
            var windowManager = new WindowManager(applicationInfo, uiSynchronization, objectFactory, viewLocator) as IWindowManager;

            RegisterService(applicationInfo);
            RegisterService(uiSynchronization);
            RegisterService(splashScreenManager);
            RegisterService(assemblyFinder);
            RegisterService(assemblyLoader);
            RegisterService(moduleFinder);
            RegisterService(container);
            RegisterService(moduleRegister);
            RegisterService(objectFactory);
            RegisterService(moduleScheduler);
            RegisterService(moduleLoader);
            RegisterService(viewModelLocator);
            RegisterService(viewLocator);
            RegisterService(windowManager);
            RegisterService(messenger);
            RegisterService(menuItemFactory);
            RegisterService(menuBuilder);

            IoC.Initialize(container);
        }
    }
}
