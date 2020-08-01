using SLStudio.Logging;
using System;

namespace SLStudio.Core.Services
{
    internal class DefaultErrorHandler : IErrorHandler
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultErrorHandler>();

        public void HandleError(Exception exception)
        {
            logger.Error(exception);
            var output = IoC.Get<IOutput>();
            var shell = IoC.Get<IShell>();
            if (output != null)
            {
                shell.OpenPanel(output);
                output.AppendLine($"{exception.Message}{Environment.NewLine}{exception}");
            }
        }
    }
}