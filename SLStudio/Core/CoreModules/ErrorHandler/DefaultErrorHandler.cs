using System;

namespace SLStudio.Core.CoreModules.ErrorHandler
{
    internal class DefaultErrorHandler : IErrorHandler
    {
        private readonly ILogger logger;

        public DefaultErrorHandler(ILoggingFactory loggingService)
        {
            logger = loggingService.GetLoggerFor<DefaultErrorHandler>();
        }

        public void HandleError(Exception exception)
        {
            logger.Error(exception);
        }
    }
}
