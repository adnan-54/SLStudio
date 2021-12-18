using SimpleInjector;

namespace SLStudio;

internal class ApplicationContext : IApplicationContext
{
    private readonly Container globalContainer;
    private readonly Container internalContainer;

    public ApplicationContext()
    {
    }
}