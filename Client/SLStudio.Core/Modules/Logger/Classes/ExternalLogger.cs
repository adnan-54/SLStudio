namespace SLStudio.Logger;

internal class ExternalLogger : ILogger
{
    private readonly ILogManager logManager;
    private readonly ILogger internalLogger;
    private readonly ILogsRepository logsRepository;

    public ExternalLogger(ILogManager logManager, ILogger internalLogger, ILogsRepository logsRepository, string name)
    {
        this.logManager = logManager;
        this.internalLogger = internalLogger;
        this.logsRepository = logsRepository;

        Name = name;
    }

    public string Name { get; }

    public void Log(object message)
    {
        Log(message, logManager.Configuration.DefaultLogLevel);
    }

    public async void Log(object message, LogLevel? level)
    {
        if (string.IsNullOrEmpty(message?.ToString()))
            return;

        try
        {
            var log = new Log(Guid.NewGuid(), Name, message.ToString()!, level.GetValueOrDefault(logManager.Configuration.DefaultLogLevel), DateTime.Now, Environment.StackTrace.Trim());
            await logsRepository.AddLog(log);
            logManager.OnLogAdded(log);
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }
    }
}
