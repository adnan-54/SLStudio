namespace SLStudio;

internal interface IContainerHost
{
    IContainer Container { get; }

    IServiceProvider ServiceProvider { get; }
}
