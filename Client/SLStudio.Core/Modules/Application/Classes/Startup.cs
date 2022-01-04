using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

internal class Startup
{
    private readonly IApplication application;
    private readonly ILogManager logManager;

    public Startup(IApplication application, ILogManager logManager)
    {
        this.application = application;
        this.logManager = logManager;
    }

    public async Task Configure()
    {
        var builder = new ContainerBuilder();
        RegisterServices(builder);
        var container = builder.Build();
        
        ConfigureLogger();

        //Load Modules from ./SLStudio.*.dll
        //Load Modules from ./Modules/*.dll
        //Parse Commandline Args
        //Load Settings
        //Load Language
        //Load Theme

        container.Verify();
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        builder.AddService();
    }

    private void ConfigureLogger()
    {
    }
}
