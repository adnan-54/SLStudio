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

        var assemblyLoader = serviceProvider.GetService<IAssemblyLoader>()!;
        var moduleLoader = serviceProvider.GetService<IModuleLoader>()!;

        assemblyLoader.LoadAssemblies();
        moduleLoader.LoadModules();

        container.Verify();

        IoC.Initialize(container);

        //Todo:
        //Load Settings
        //Load Language
        //Load Theme

        var logManager = serviceProvider.GetService<ILogManager>()!;
        logManager.Initialize(new(LogLevel.Information, LogLevel.Debug));

        //Todo:
        //Run task scheduler
    }
}
