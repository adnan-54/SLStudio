namespace SLStudio;

internal class SLStudio : IApplication
{
    private readonly Application application;

    public SLStudio()
    {
        application = new();
    }

    public int Run()
    {
        return application.Run();
    }

}

public interface IApplication
{
    IWindowViewModel MainWindow { get; }

    IWindowViewModel CurrentWindow { get; }

    IEnumerable<IWindowViewModel> Windows { get; }

    int Run();

    void Shutdown(int exitCode);

    bool TryFindResource(object resourceKey, out object resource);
}