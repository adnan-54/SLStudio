namespace SLStudio.Logging
{
    public class LogManagerConfiguration
    {
        public LogLevel DefaultLogLevel { get; init; }

        public bool IgnoreDebugLevel { get; init; }

        public bool LogToConsole { get; init; }

        public int MaxRetrieveResults { get; init; }
    }

    internal class DefaultLogManagerConfiguration : LogManagerConfiguration
    {
        public DefaultLogManagerConfiguration()
        {
            DefaultLogLevel = LogLevel.Info;
            IgnoreDebugLevel = true;
            LogToConsole = true;
            MaxRetrieveResults = 1000;
        }
    }
}