using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    public static class LogManager
    {
        private static readonly Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();

        private static readonly LoggingService loggingService = new LoggingService();

        public static event EventHandler<LogCompletedEventArgs> LogCompleted;

        public static LogManagerConfiguration Configuraion { get; private set; } = new DefaultLogManagerConfiguration();

        public static void Configure(LogManagerConfiguration configuration)
        {
            Configuraion = configuration;
        }

        public static Task<DataTable> GetLogs()
        {
            return loggingService.GetLogs();
        }

        public static ILogger GetLoggerFor(string name)
        {
            if (!loggers.TryGetValue(name, out ILogger logger))
            {
                logger = new Logger(name, loggingService);
                loggers.Add(name, logger);
            }

            return logger;
        }

        public static ILogger GetLogger(Type type)
        {
            return GetLoggerFor(type.Name);
        }

        public static ILogger GetLoggerFor<Type>() where Type : class
        {
            return GetLogger(typeof(Type));
        }

        /// <summary>
        /// Get the logs database file size in bytes
        /// </summary>
        /// <returns>The database size in bytes</returns>
        public static long GetLogFileSize()
        {
            return loggingService.GetLogFileSize();
        }

        public static Task<string> GetSimpleLog()
        {
            return loggingService.GetSimpleLogFile();
        }

        public static Task ClearAll()
        {
            return loggingService.ClearAll();
        }

        internal static void OnLogCompleted(Log log)
        {
            LogCompleted?.Invoke(loggingService, new LogCompletedEventArgs(log));
        }
    }
}