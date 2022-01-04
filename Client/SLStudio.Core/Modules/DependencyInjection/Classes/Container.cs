using System.ComponentModel.Design;

namespace SLStudio;

internal class Container : IContainer
{
    private readonly IServiceProvider serviceProvider;
    private readonly SimpleInjector.Container container;

    public Container(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        container = new();
    }

    public bool IsLocked => container.IsLocked;

    public IServiceProvider Services => IsLocked ? this : serviceProvider;

    public void Verify()
    {
        CheckIsLocked();
        container.Verify();
    }

    IContainerContext IContainerContext.AddTransient(Type concreteType)
    {
        CheckIsLocked();
        container.Register(concreteType);
        return this;
    }

    IContainerContext IContainerContext.AddTransient(Type serviceType, Func<object> instanceCreator)
    {
        CheckIsLocked();
        container.Register(serviceType, instanceCreator);
        return this;
    }

    IContainerContext IContainerContext.AddTransient<TConcrete>()
    {
        CheckIsLocked();
        container.Register<TConcrete>();
        return this;
    }

    IContainerContext IContainerContext.AddTransient<TService, TImplementation>()
    {
        CheckIsLocked();
        container.Register<TService, TImplementation>();
        return this;
    }

    IContainerContext IContainerContext.AddTransient<TService>(Func<TService> instanceCreator)
    {
        CheckIsLocked();
        container.Register(instanceCreator);
        return this;
    }

    IContainerContext IContainerContext.AddSingleton(Type serviceType, Type implementationType)
    {
        CheckIsLocked();
        container.Register(serviceType, implementationType);
        return this;
    }

    IContainerContext IContainerContext.AddSingleton(Type serviceType, Func<object> instanceCreator)
    {
        CheckIsLocked();
        container.RegisterSingleton(serviceType, instanceCreator);
        return this;
    }

    IContainerContext IContainerContext.AddSingleton<TConcrete>()
    {
        CheckIsLocked();
        container.RegisterSingleton<TConcrete>();
        return this;
    }

    IContainerContext IContainerContext.AddSingleton<TService, TImplementation>()
    {
        CheckIsLocked();
        container.RegisterSingleton<TService, TImplementation>();
        return this;
    }

    IContainerContext IContainerContext.AddSingleton<TService>(Func<TService> instanceCreator)
    {
        CheckIsLocked();
        container.RegisterSingleton(instanceCreator);
        return this;
    }

    IContainerContext IContainerContext.AddDecorator(Type serviceType, Type decoratorType)
    {
        CheckIsLocked();
        container.RegisterDecorator(serviceType, decoratorType);
        return this;
    }

    IContainerContext IContainerContext.AddDecorator<TService, TDecorator>()
    {
        CheckIsLocked();
        container.RegisterDecorator<TService, TDecorator>();
        return this;
    }

    IContainerContext IContainerContext.AddInstance(Type serviceType, object instance)
    {
        CheckIsLocked();
        container.RegisterInstance(serviceType, instance);
        return this;
    }

    IContainerContext IContainerContext.AddInstance<TService>(TService instance)
    {
        CheckIsLocked();
        container.RegisterInstance(instance);
        return this;
    }

    IContainerContext IContainerContext.AddInitializer<TService>(Action<TService> instanceInitializer)
    {
        CheckIsLocked();
        container.RegisterInitializer(instanceInitializer);
        return this;
    }

    IContainerContext IContainerContext.AddCustom(Action<SimpleInjector.Container> action)
    {
        action?.Invoke(container);
        return this;
    }

    object? IServiceProvider.GetService(Type serviceType)
    {
        return Services.GetService(serviceType);
    }

    public object GetObject(Type serviceType)
    {
        CheckIsNotLocked();
        return container.GetInstance(serviceType);
    }

    public TService GetObject<TService>() where TService : class
    {
        CheckIsNotLocked();
        return container.GetInstance<TService>();
    }

    public IEnumerable<object> GetAllObjects(Type serviceType)
    {
        CheckIsNotLocked();
        return container.GetAllInstances(serviceType);
    }

    public IEnumerable<TService> GetAllObjects<TService>() where TService : class
    {
        CheckIsNotLocked();
        return container.GetAllInstances<TService>();
    }

    public ValueTask DisposeAsync()
    {
        return container.DisposeAsync();
    }

    public void Dispose()
    {
        container.Dispose();
    }

    private void CheckIsLocked()
    {
        if (IsLocked)
            throw new InvalidOperationException("Container is already locked");
    }

    private void CheckIsNotLocked()
    {
        if (!IsLocked)
            throw new InvalidOperationException("Container is not locked yet");
    }   
}
