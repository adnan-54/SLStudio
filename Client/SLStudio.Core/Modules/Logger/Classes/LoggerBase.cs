

namespace SLStudio.Logger;

internal abstract class LoggerBase : ILogger
{
    private readonly ILogManager logManager;

    protected LoggerBase(ILogManager logManager, string name)
    {
        this.logManager = logManager;
        Name = name;
    }

    public string Name { get; }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= logManager.Configuration.MinimumLogLevel;
    }

    public async void Log(string message, LogLevel level)
    {
        if (string.IsNullOrEmpty(message) || !IsEnabled(level))
            return;

        try
        {
            var log = new Log(Guid.NewGuid(), Name, message, level, DateTime.Now, Environment.StackTrace);
            await Write(log);
            logManager.OnLogAdded(log);
        }
        catch { }
    }

    protected abstract Task Write(Log log);
}