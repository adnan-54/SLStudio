namespace SLStudio.Logging
{
    internal class DefaultConfigurationService : IConfigurationService
    {
        private LoggerConfiguration configuration;

        public DefaultConfigurationService()
        {
            configuration = new DefaultLoggerConfiguration();
        }

        public void Initialize(LoggerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public LogLevel DefaultLogLevel => configuration.DefaultLogLevel;

        public LogLevel MinimumLogLevel => configuration.MinimumLogLevel;

        public bool IgnoreDebugLevel => configuration.IgnoreDebugLevel;

        public bool LogToConsole => configuration.LogToConsole;

        public bool LogToDebug => configuration.LogToDebug;

        public int MaxRetrieveResults => configuration.MaxRetrieveResults;
    }

    internal interface IConfigurationService
    {
        LogLevel DefaultLogLevel { get; }

        LogLevel MinimumLogLevel { get; }

        bool IgnoreDebugLevel { get; }

        bool LogToConsole { get; }

        bool LogToDebug { get; }

        int MaxRetrieveResults { get; }

        void Initialize(LoggerConfiguration configuration);
    }
}