using Caliburn.Micro;
using SLStudio.Core.Utilities.Logger;
using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    internal class DefaultLoggingFactory : ILoggingFactory
    {
        private readonly ILoggingService loggingService;
        private readonly ICommandLineArguments commandLineArguments;
        private readonly Dictionary<Type, ILogger> logs;

        public DefaultLoggingFactory(ILoggingService loggingService, ICommandLineArguments commandLineArguments)
        {
            this.loggingService = loggingService;
            this.commandLineArguments = commandLineArguments;
            logs = new Dictionary<Type, ILogger>();
        }

        public ILogger GetLogger<Type>() where Type : class
        {
            if (!logs.TryGetValue(typeof(Type), out ILogger logger))
            {
                logger = new DefaultLogger(typeof(Type), loggingService, commandLineArguments);
                logs.Add(typeof(Type), logger);
            }

            return logger;
        }
    }

    public static class LogManager
    {
        public static ILogger GetLogger<Type>() where Type : class
        {
            var logger = IoC.Get<ILoggingFactory>()?.GetLogger<Type>();
            return logger;
        }
    }
}