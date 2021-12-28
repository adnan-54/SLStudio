using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

internal class Container : IContainer
{
    private readonly SimpleInjector.Container container;
    private readonly IServiceCollection services;


    public Container()
    {
        container = new();
        services = new ServiceCollection();
        services.BuildServiceProvider();
    }

    public bool IsLocked { get; }

    public IServiceProvider Services { get; }

    public void Verify()
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddTransient(Type concreteType)
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddTransient(Type serviceType, Func<object> instanceCreator)
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddTransient<TConcrete>() where TConcrete : class
    {
        throw new NotImplementedException();
    }

    IContainerContext IContainerContext.AddTransient<TService, TImplementation>()
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddTransient<TService>(Func<TService> instanceCreator) where TService : class
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddSingleton(Type serviceType, Type implementationType)
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddSingleton(Type serviceType, Func<object> instanceCreator)
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddSingleton<TConcrete>() where TConcrete : class
    {
        throw new NotImplementedException();
    }

    IContainerContext IContainerContext.AddSingleton<TService, TImplementation>()
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddSingleton<TService>(Func<TService> instanceCreator) where TService : class
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddDecorator(Type serviceType, Type decoratorType)
    {
        throw new NotImplementedException();
    }

    IContainerContext IContainerContext.AddDecorator<TService, TDecorator>()
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddInstance<TService>(TService instance) where TService : class
    {
        throw new NotImplementedException();
    }

    public IContainerContext AddInitializer<TService>(Action<TService> instanceInitializer) where TService : class
    {
        throw new NotImplementedException();
    }

    public object GetObject(Type serviceType)
    {
        throw new NotImplementedException();
    }

    public TService GetObject<TService>() where TService : class
    {
        throw new NotImplementedException();
    }

    public IEnumerable<object> GetAllObjects(Type serviceType)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TService> GetAllObjects<TService>() where TService : class
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    IContainerContext IContainerContext.AddCustom(Action<SimpleInjector.Container> action)
    {
        action?.Invoke(container);
        return this;
    }
}


class ContainerBuilder
{

    public IContainer Build()
    {

    }
}