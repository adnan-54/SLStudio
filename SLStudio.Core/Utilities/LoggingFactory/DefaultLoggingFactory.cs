using SLStudio.Core.Utilities.Logger;

namespace SLStudio.Core
{
    internal class DefaultLoggingFactory : ILoggingFactory
    {
        private readonly ILoggingService loggingService;
        private readonly ICommandLineArguments commandLineArguments;

        public DefaultLoggingFactory(ILoggingService loggingService, ICommandLineArguments commandLineArguments)
        {
            this.loggingService = loggingService;
            this.commandLineArguments = commandLineArguments;
        }

        public ILogger GetLoggerFor<Type>() where Type : class
        {
            return new DefaultLogger(typeof(Type), loggingService, commandLineArguments);
        }
    }
}