using SLStudio.Logging;
using System;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultErrorHandler : IErrorHandler
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultErrorHandler>();

        public void HandleError(Exception exception)
        {
            HandleErrorAsync(exception).FireAndForget();
        }

        private static async Task HandleErrorAsync(Exception exception)
        {
            logger.Error(exception);
            var shell = IoC.Get<IShell>();
            if (shell != null)
            {
                var output = await shell.OpenWorkspace<IOutput>();
                output.AppendLine($"{exception.Message}{Environment.NewLine}{exception}");
            }
        }
    }

    public interface IErrorHandler
    {
        void HandleError(Exception exception);
    }
}