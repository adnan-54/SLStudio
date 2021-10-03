using System;
using SLStudio.Logging;

namespace SLStudio
{
    internal abstract class ErrorHandler
    {
        private static readonly ILogger logger = LogManager.GetLogger<ErrorHandler>();

        public void HandleError(Exception ex)
        {
            try
            {
                Handle(ex);
            }
            catch (Exception exception)
            {
                try
                {
                    logger.Fatal(ex, "Failed to handle error");
                    logger.Fatal(exception, "Failed to handle error");
                }
                catch { }
            }
        }

        protected abstract void Handle(Exception exception);
    }
}