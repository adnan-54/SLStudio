namespace SLStudio;

public static class IoC
{
    private static IContainer? container;

    public static bool IsInitialized => container is not null;

    public static IEnumerable<object> GetAll(Type servicesType)
    {
        CheckInitialized();
        return container!.GetAllInstances(servicesType);
    }

    public static IEnumerable<TService> GetAll<TService>()
        where TService : class
    {
        CheckInitialized();
        return container!.GetAllInstances<TService>();
    }

    public static object Get(Type serviceType)
    {
        CheckInitialized();
        return container!.GetInstance(serviceType);
    }

    public static TService Get<TService>()
        where TService : class
    {
        CheckInitialized();
        return container!.GetInstance<TService>();
    }

    internal static void Initialize(IContainer container)
    {
        if (IsInitialized)
            throw new InvalidOperationException($"{nameof(IoC)} is already initialized");

        IoC.container = container;
    }

    private static void CheckInitialized()
    {
        if (!IsInitialized)
            throw new InvalidOperationException($"{nameof(IoC)} is not initialized");
    }
}
