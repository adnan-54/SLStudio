using SLStudio.Core.Logging;
using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public static class LogManager
    {
        private static readonly Dictionary<Type, ILogger> logs = new Dictionary<Type, ILogger>();
        private static readonly DefaultLoggingService loggingService = new DefaultLoggingService();

        public static ILogger GetLogger(Type type)
        {
            if (!logs.TryGetValue(typeof(Type), out ILogger logger))
            {
                logger = new DefaultLogger(type, loggingService);
                logs.Add(type, logger);
            }

            return logger;
        }

        public static readonly ILoggingService LoggingService = loggingService;
    }
}