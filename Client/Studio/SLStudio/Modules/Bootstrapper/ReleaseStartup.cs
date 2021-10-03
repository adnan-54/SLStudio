using SLStudio.ErrorReporting;
using SLStudio.LocalServer;
using SLStudio.Logging;
using System;
using System.Threading;

namespace SLStudio
{
    internal class ReleaseStartup : Startup
    {
        private Mutex mutex;
        private bool mutexInitialized;

        public ReleaseStartup()
        {
            var errorReportingService = ErrorReportingModule.ErrorReportingService;
            ErrorHandler = new ReleaseErrorHandler(errorReportingService);
        }

        public override ErrorHandler ErrorHandler { get; }

        protected override int Run()
        {
            try
            {
                mutex = new Mutex(true, SharedConstants.GlobalKey, out mutexInitialized);

                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
                {
                    NotifyServer();
                    LogManager.RequestDump();
                    return 1;
                }

                var server = Server.CreateServer();
                server.Start();

                return RunApplication();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return ex.HResult;
            }
            finally
            {
                try
                {
                    if (mutexInitialized)
                        mutex?.ReleaseMutex();

                    mutex?.Dispose();
                }
                catch { }
            }
        }

        private void NotifyServer()
        {
            var message = new ClientMessage() { Args = Args };
            Client.SendMessage(message);
        }
    }
}