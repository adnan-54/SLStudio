using System.Collections.Generic;
using System.Data;

namespace SLStudio.Logging
{
    public static class LogManager
    {
        public static bool IsInitialized { get; private set; } = false;

        private static List<Logger> loggers = new List<Logger>();

        public static ILog GetLogger(string name)
        {
            if (!IsInitialized)
                Initialize();

            foreach (Logger log in loggers)
            {
                if (log.Name == name)
                    return log;
            }

            Logger newLogger = new Logger(name);
            loggers.Add(newLogger);

            return newLogger;
        }

        public static void Initialize()
        {
            if (IsInitialized)
                return;

            InternalLogManager internalManager = InternalLogManager.GetInstance();

            if (!internalManager.LogDirectoryExists())
                internalManager.CreateLogDirectory();

            if (!internalManager.LogFileExists())
                internalManager.CreateLogFile();

            IsInitialized = true;
        }

        public static DataTable GetLog()
        {
            InternalLogManager internalManager = InternalLogManager.GetInstance();

            return internalManager.GetLog();
        }

        public static void ExportLogToHtml()
        {

        }
    }
}
