namespace SLStudio.Logger;

internal class InternalLogger : LoggerBase
{
    public InternalLogger(ILogManager logManager) : base(logManager, "Internal Logger") { }

    protected override Task Write(Log log)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(LogManager.InternalLoggerSeparator);
        stringBuilder.AppendLine(log.ToString());
        stringBuilder.AppendLine(LogManager.InternalLoggerSeparator);

        return File.AppendAllTextAsync(LogManager.LogsOutputFile, stringBuilder.ToString());
    }
}
