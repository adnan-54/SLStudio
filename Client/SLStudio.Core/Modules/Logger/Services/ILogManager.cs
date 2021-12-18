namespace SLStudio.Logger;

public interface ILogManager
{
    event EventHandler Initialized;

    event EventHandler<Log> LogAdded;

    LoggerConfiguration Configuration { get; }

    bool IsInitialized { get; }

    void Initialize(LoggerConfiguration loggerConfiguration);

    ILogger GetLogger(string? name);

    Task Dump();

    internal void OnLogAdded(Log log);
}
