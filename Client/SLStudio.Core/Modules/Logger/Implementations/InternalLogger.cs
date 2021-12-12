

namespace SLStudio.Logger;

internal class InternalLogger : ILogger
{
    private readonly ILogManager logManager;

    public InternalLogger(ILogManager logManager)
    {
        this.logManager = logManager;
        Name = "Internal Logger";
    }

    public string Name { get; }

    public void Log(object message)
    {
        Log(message, null);
    }

    public async void Log(object message, LogLevel? level)
    {
        if (string.IsNullOrEmpty(message?.ToString()))
            return;

        try
        {
            var log = new Log(Guid.NewGuid(), Name, message.ToString()!, LogLevel.Error, DateTime.Now, Environment.StackTrace);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(LogManager.InternalLoggerSeparator);
            stringBuilder.AppendLine(log.ToString());
            stringBuilder.AppendLine(LogManager.InternalLoggerSeparator);

            await File.AppendAllTextAsync(LogManager.LogsOutputFile, stringBuilder.ToString());

            logManager.OnLogAdded(log);
        }
        catch { }
    }
}
