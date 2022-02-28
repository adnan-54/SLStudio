namespace SLStudio;

public interface IConfigurationContext : IServiceProvider
{
    IConfigurationContext AddTransient(Type concreteType);

    IConfigurationContext AddTransient(Type serviceType, Func<object> instanceCreator);

    IConfigurationContext AddTransient<TConcrete>()
        where TConcrete : class;

    IConfigurationContext AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IConfigurationContext AddTransient<TService>(Func<TService> instanceCreator)
        where TService : class;

    IConfigurationContext AddSingleton(Type serviceType, Type implementationType);

    IConfigurationContext AddSingleton(Type serviceType, Func<object> instanceCreator);

    IConfigurationContext AddSingleton<TConcrete>()
        where TConcrete : class;

    IConfigurationContext AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IConfigurationContext AddSingleton<TService>(Func<TService> instanceCreator)
        where TService : class;

    IConfigurationContext AddDecorator(Type serviceType, Type decoratorType);

    IConfigurationContext AddDecorator<TService, TDecorator>()
        where TService : class
        where TDecorator : class, TService;

    IConfigurationContext AddInstance(Type serviceType, object instance);

    IConfigurationContext AddInstance<TService>(TService instance)
        where TService : class;

    IConfigurationContext AddInitializer<TService>(Action<TService> instanceInitializer)
        where TService : class;

    IConfigurationContext AddSubModule<TSubModule>()
        where TSubModule : class, ISubModule, new();

    internal IConfigurationContext AddCustom(Action<IContainer> action);
}
