namespace SLStudio;

public interface IContainer : IContainerContext, IObjectFactory, IServiceProvider, IAsyncDisposable, IDisposable
{
    bool IsLocked { get; }

    void Verify();
}

public interface IContainerContext
{
    IContainerContext AddTransient(Type concreteType);

    IContainerContext AddTransient(Type serviceType, Func<object> instanceCreator);

    IContainerContext AddTransient<TConcrete>()
        where TConcrete : class;

    IContainerContext AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IContainerContext AddTransient<TService>(Func<TService> instanceCreator)
        where TService : class;

    IContainerContext AddSingleton(Type serviceType, Type implementationType);

    IContainerContext AddSingleton(Type serviceType, Func<object> instanceCreator);

    IContainerContext AddSingleton<TConcrete>()
        where TConcrete : class;

    IContainerContext AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IContainerContext AddSingleton<TService>(Func<TService> instanceCreator)
        where TService : class;

    IContainerContext AddDecorator(Type serviceType, Type decoratorType);

    IContainerContext AddDecorator<TService, TDecorator>()
        where TService : class
        where TDecorator : class, TService;

    IContainerContext AddInstance<TService>(TService instance)
        where TService : class;

    IContainerContext AddInitializer<TService>(Action<TService> instanceInitializer)
        where TService : class;

    internal IContainerContext AddCustom(Action<SimpleInjector.Container> action);
}

public interface IObjectFactory
{
    object GetObject(Type serviceType);

    TService GetObject<TService>()
        where TService : class;

    IEnumerable<object> GetAllObjects(Type serviceType);

    IEnumerable<TService> GetAllObjects<TService>()
        where TService : class;
}