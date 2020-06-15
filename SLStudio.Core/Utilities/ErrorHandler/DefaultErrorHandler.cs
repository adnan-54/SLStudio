using SLStudio.Core.Logging;
using System;

namespace SLStudio.Core.Utilities.ErrorHandler
{
    internal class DefaultErrorHandler : IErrorHandler
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(DefaultErrorHandler));

        public void HandleError(Exception exception)
        {
            logger.Error(exception);
        }
    }
}