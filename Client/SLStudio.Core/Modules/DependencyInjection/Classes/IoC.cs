using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

public static class IoC
{
    private static IServiceProvider? Provider;

    private static bool IsLocked;

    public static IEnumerable<object?> GetAll(Type servicesType)
    {
        CheckInitialized();
        return Provider!.GetServices(servicesType);
    }

    public static IEnumerable<TService> GetAll<TService>()
        where TService : class
    {
        CheckInitialized();
        return Provider!.GetServices<TService>();
    }

    public static object Get(Type serviceType)
    {
        CheckInitialized();
        return Provider!.GetService(serviceType) ?? throw new Exception($"Service '{serviceType}' not foud ");
    }

    public static TService Get<TService>()
        where TService : class
    {
        CheckInitialized();
        return Provider!.GetService<TService>() ?? throw new Exception($"Service '{typeof(TService)}' not foud ");
    }

    internal static void SetProvider(IServiceProvider serviceProvider)
    {
        if (IsLocked)
            throw new InvalidOperationException($"{nameof(IoC)} container is already locked");
        Provider = serviceProvider;
    }

    internal static void Lock()
    {
        if (Provider is null)
            throw new InvalidOperationException($"{nameof(IoC)} cannot be locked without a service provider");
        IsLocked = true;
    }

    private static void CheckInitialized()
    {
        if (Provider is null)
            throw new InvalidOperationException($"{nameof(IoC)} does not have a service provider");
    }
}
