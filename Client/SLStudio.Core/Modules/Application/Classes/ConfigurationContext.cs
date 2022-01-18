namespace SLStudio;

internal class ConfigurationContext : IConfigurationContext
{
    private readonly IContainer container;
    private readonly IServiceProvider services;

    public ConfigurationContext(IContainer container, IServiceProvider services)
    {
        this.container = container;
        this.services = services;
    }

    IConfigurationContext IConfigurationContext.AddTransient(Type concreteType)
    {
        container.Register(concreteType);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddTransient(Type serviceType, Func<object> instanceCreator)
    {
        container.Register(serviceType, instanceCreator);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddTransient<TConcrete>()
    {
        container.Register<TConcrete>();
        return this;
    }

    IConfigurationContext IConfigurationContext.AddTransient<TService, TImplementation>()
    {
        container.Register<TService, TImplementation>();
        return this;
    }

    IConfigurationContext IConfigurationContext.AddTransient<TService>(Func<TService> instanceCreator)
    {
        container.Register(instanceCreator);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddSingleton(Type serviceType, Type implementationType)
    {
        container.RegisterSingleton(serviceType, implementationType);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddSingleton(Type serviceType, Func<object> instanceCreator)
    {
        container.RegisterSingleton(serviceType, instanceCreator);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddSingleton<TConcrete>()
    {
        container.RegisterSingleton<TConcrete>();
        return this;
    }

    IConfigurationContext IConfigurationContext.AddSingleton<TService, TImplementation>()
    {
        container.RegisterSingleton<TService, TImplementation>();
        return this;
    }

    IConfigurationContext IConfigurationContext.AddSingleton<TService>(Func<TService> instanceCreator)
    {
        container.RegisterSingleton(instanceCreator);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddDecorator(Type serviceType, Type decoratorType)
    {
        container.RegisterDecorator(serviceType, decoratorType);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddDecorator<TService, TDecorator>()
    {
        container.RegisterDecorator<TService, TDecorator>();
        return this;
    }

    IConfigurationContext IConfigurationContext.AddInstance(Type serviceType, object instance)
    {
        container.RegisterInstance(serviceType, instance);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddInstance<TService>(TService instance)
    {
        container.RegisterInstance(instance);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddInitializer<TService>(Action<TService> instanceInitializer)
    {
        container.RegisterInitializer(instanceInitializer);
        return this;
    }

    IConfigurationContext IConfigurationContext.AddCustom(Action<IContainer> action)
    {
        action?.Invoke(container);
        return this;
    }

    object? IServiceProvider.GetService(Type serviceType)
    {
        return services.GetService(serviceType);
    }
}