using System;
using System.Collections.Generic;

namespace SLStudio.Logging
{
    public static class LogManager
    {
        private static readonly Dictionary<Type, ILogger> loggers = new Dictionary<Type, ILogger>();
        private static readonly LoggingService loggingService = new LoggingService();

        private static bool alreadyInitialized = false;

        internal static LogLevel DefaultLogLevel { get; private set; }

        internal static bool IgnoreDebugLevel { get; private set; }

        internal static bool LogToConsole { get; private set; }

        public static event EventHandler<LogCompletedEventArgs> LogCompleted;

        public static void Initialize(LogLevel defaultLogLevel, bool ignoreDebugLevel, bool logToConsole)
        {
            if (alreadyInitialized)
                return;
            alreadyInitialized = true;

            DefaultLogLevel = defaultLogLevel;
            IgnoreDebugLevel = ignoreDebugLevel;
            LogToConsole = logToConsole;
        }

        public static ILogger GetLogger(Type type)
        {
            if (!loggers.TryGetValue(type, out ILogger logger))
            {
                logger = new Logger(type.Name, loggingService);
                loggers.Add(type, logger);
            }

            return logger;
        }

        public static ILogger GetLoggerFor<Type>() where Type : class
        {
            return GetLogger(typeof(Type));
        }

        internal static void OnLogCompleted(Log log)
        {
            LogCompleted?.Invoke(loggingService, new LogCompletedEventArgs(log));
        }
    }
}