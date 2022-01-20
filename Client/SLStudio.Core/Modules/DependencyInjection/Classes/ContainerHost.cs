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
    private readonly IShell shell;
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
        shell = new ShellViewModel();
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
        container.RegisterInstance(shell);
        container.RegisterInstance(messenger);
    }
}