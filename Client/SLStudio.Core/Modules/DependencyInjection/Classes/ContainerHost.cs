namespace SLStudio;

internal class ContainerHost : IContainerHost
{
    private readonly IApplication application;
    private readonly ILogManager logManager;
    private readonly IContainer container;
    private readonly IContainer serviceProvider;
    private readonly IConfigurationContext configurationContext;
    private readonly IAssemblyLoader assemblyLoader;
    private readonly IModuleLoader moduleLoader;
    private readonly IObjectFactory objectFactory;

    public ContainerHost(IApplication application, ILogManager logManager)
    {
        this.application = application;
        this.logManager = logManager;

        container = new Container();
        serviceProvider = new Container();
        configurationContext = new ConfigurationContext(container, serviceProvider);

        assemblyLoader = new AssemblyLoader();
        moduleLoader = new ModuleLoader(configurationContext);
        objectFactory = new ObjectFactory(container);

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
    }
}