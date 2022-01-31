using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

internal class Startup
{
    private readonly IContainerHost host;

    public Startup(IContainerHost host)
    {
        this.host = host;
    }

    public void Configure()
    {
        var container = host.Container;
        var serviceProvider = host.ServiceProvider;
        IoC.SetProvider(serviceProvider);

        var assemblyLoader = serviceProvider.GetService<IAssemblyLoader>();
        var moduleLoader = serviceProvider.GetService<IModuleLoader>();

        assemblyLoader?.LoadAssemblies();
        moduleLoader?.LoadModules();

        //Todo:
        //Load Settings
        //Load Language
        //Load Theme

        var logManager = serviceProvider.GetService<ILogManager>()!;
        logManager.Initialize(new(LogLevel.Information, LogLevel.Debug));

        container.Verify();
        IoC.SetProvider(container);
        IoC.Lock();

        //Todo:
        //Run task scheduler
    }
}
