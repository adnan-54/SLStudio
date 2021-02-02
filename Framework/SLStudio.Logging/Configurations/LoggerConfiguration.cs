namespace SLStudio.Logging
{
    public class LoggerConfiguration
    {
        public LogLevel DefaultLogLevel { get; init; }

        public LogLevel MinimumLogLevel { get; init; }

        public bool IgnoreDebugLevel { get; init; }

        public bool LogToConsole { get; init; }

        public bool LogToDebug { get; init; }

        public int MaxRetrieveResults { get; init; }
    }
}