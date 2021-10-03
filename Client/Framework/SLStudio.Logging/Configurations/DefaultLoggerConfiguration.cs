namespace SLStudio.Logging
{
    internal class DefaultLoggerConfiguration : LoggerConfiguration
    {
        public DefaultLoggerConfiguration()
        {
            DefaultLogLevel = LogLevel.Info;
            MinimumLogLevel = LogLevel.Info;
            IgnoreDebugLevel = true;
            LogToConsole = true;
            LogToDebug = false;
            MaxRetrieveResults = 1000;
        }
    }
}