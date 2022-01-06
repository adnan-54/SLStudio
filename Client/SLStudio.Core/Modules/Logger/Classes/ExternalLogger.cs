namespace SLStudio.Logger;

internal class ExternalLogger : LoggerBase
{
    private readonly ILogger internalLogger;
    private readonly ILogsRepository logsRepository;

    public ExternalLogger(ILogManager logManager, ILogger internalLogger, ILogsRepository logsRepository, string name) : base(logManager, name)
    {
        this.internalLogger = internalLogger;
        this.logsRepository = logsRepository;
    }

    protected override async Task Write(Log log)
    {
        try
        {
            await logsRepository.AddLog(log);
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
            throw;
        }
    }
}
