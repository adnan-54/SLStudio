namespace SLStudio;

public interface IApplication
{
    string[] Args { get; }

    IDispatcher Dispatcher { get; }

    IWindowViewModel MainWindow { get; }

    IWindowViewModel CurrentWindow { get; }

    IEnumerable<IWindowViewModel> Windows { get; }

    int Run();

    void Shutdown(int exitCode);

    bool TryFindResource(object resourceKey, out object resource);

    void LoadResource(Uri path);
}
