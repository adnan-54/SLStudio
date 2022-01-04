using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

internal class ContainerBuilder
{
    private readonly IServiceCollection services;

    public ContainerBuilder()
    {
        services = new ServiceCollection();
    }

    public void AddService<TService>(TService instance) where TService : class
    {
        if(instance is null)
            throw new ArgumentNullException(nameof(instance));

        services.Add(new(typeof(TService), instance));
    }

    public IContainer Build()
    {
        var provider = services.BuildServiceProvider();
        var container = new Container(provider) as IContainer;

        foreach(var registration in services)
        {
            var type = registration.ServiceType;
            var instance = registration.ImplementationInstance!;
            container.AddInstance(type, instance);
        }

        return container;
    }
}