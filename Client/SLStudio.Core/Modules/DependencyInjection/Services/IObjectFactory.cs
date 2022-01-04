namespace SLStudio;

public interface IObjectFactory
{
    object GetObject(Type serviceType);

    TService GetObject<TService>()
        where TService : class;

    IEnumerable<object> GetAllObjects(Type serviceType);

    IEnumerable<TService> GetAllObjects<TService>()
        where TService : class;
}