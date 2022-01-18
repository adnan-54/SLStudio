namespace SLStudio;

internal class ObjectFactory : IObjectFactory
{
    private readonly IContainer container;

    public ObjectFactory(IContainer container)
    {
        this.container = container;
    }

    public object GetObject(Type serviceType)
    {
        return container.GetInstance(serviceType);
    }

    public TService GetObject<TService>()
        where TService : class
    {
        return container.GetInstance<TService>();
    }

    public IEnumerable<object> GetAllObjects(Type serviceType)
    {
        return container.GetAllInstances(serviceType);
    }

    public IEnumerable<TService> GetAllObjects<TService>()
        where TService : class
    {
        return container.GetAllInstances<TService>();
    }
}