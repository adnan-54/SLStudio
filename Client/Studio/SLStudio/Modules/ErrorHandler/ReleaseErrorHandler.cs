using System;
using System.Windows.Threading;
using SLStudio.ErrorReporting;
using SLStudio.Logging;

namespace SLStudio
{
    internal class ReleaseErrorHandler : ErrorHandler
    {
        private static readonly ILogger logger = LogManager.GetLogger<ReleaseErrorHandler>();

        private readonly IErrorReportingService errorReportingService;

        public ReleaseErrorHandler(IErrorReportingService errorReportingService)
        {
            this.errorReportingService = errorReportingService;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Dispatcher.CurrentDispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        protected override void Handle(Exception exception)
        {
            LogException(exception);
            LogManager.RequestDump();
            ShowExceptionBox(exception, true);
            ReportException(exception);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                var isTerminating = e.IsTerminating;

                HandleException(exception, isTerminating);
            }
            catch { }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception;

                HandleException(exception, false);
            }
            catch { }
        }

        private void HandleException(Exception exception, bool isTerminating)
        {
            LogException(exception);
            ShowExceptionBox(exception, isTerminating);
            ReportException(exception);

            if (isTerminating)
            {
                logger.Fatal("Runtime is terminating because of unhandled exception");
                LogManager.RequestDump();
                Environment.FailFast("Runtime is terminating because of unhandled exception", exception);
            }
        }

        private static void LogException(Exception exception)
        {
            logger.Error(exception, "Unhandled exception");
        }

        private void ShowExceptionBox(Exception exception, bool isTerminating)
        {
            errorReportingService.ShowExceptionBox(exception, isTerminating);
        }

        private void ReportException(Exception exception)
        {
            errorReportingService.ShowReportDialog(exception);
        }
    }
}