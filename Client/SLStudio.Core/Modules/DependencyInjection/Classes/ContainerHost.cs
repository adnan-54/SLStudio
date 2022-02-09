namespace SLStudio;

internal class ContainerHost : IContainerHost
{
    private readonly IApplication application;
    private readonly IContainer container;
    private readonly IContainer serviceProvider;
    private readonly IConfigurationContext configurationContext;
    private readonly ILogManager logManager;
    private readonly IAssemblyLoader assemblyLoader;
    private readonly IModuleLoader moduleLoader;
    private readonly IObjectFactory objectFactory;
    private readonly IViewModelFactory viewModelFactory;
    private readonly IViewFactory viewFactory;
    private readonly IWindowManager windowManager;
    private readonly IMessenger messenger;

    public ContainerHost(IApplication application)
    {
        this.application = application;

        container = new Container();
        serviceProvider = new Container();
        configurationContext = new ConfigurationContext(container, serviceProvider);

        logManager = LogManager.Default;
        assemblyLoader = new AssemblyLoader();
        moduleLoader = new ModuleLoader(configurationContext);
        objectFactory = new ObjectFactory(container);
        viewModelFactory = new ViewModelFactory(objectFactory);
        viewFactory = new ViewFactory(objectFactory, viewModelFactory);
        windowManager = new WindowManager(application, viewModelFactory, viewFactory);
        messenger = Messenger.Default;

        RegisterServices(container);
        RegisterServices(serviceProvider);
        serviceProvider.Verify();
    }

    public IContainer Container => container;

    public IServiceProvider ServiceProvider => serviceProvider;

    private void RegisterServices(IContainer container)
    {
        container.RegisterInstance(Container);
        container.RegisterInstance(application);
        container.RegisterInstance(logManager);
        container.RegisterInstance(assemblyLoader);
        container.RegisterInstance(moduleLoader);
        container.RegisterInstance(objectFactory);
        container.RegisterInstance(viewModelFactory);
        container.RegisterInstance(viewFactory);
        container.RegisterInstance(windowManager);
        container.RegisterInstance(messenger);
    }
}