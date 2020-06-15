using System;

namespace SLStudio.Core.Logging
{
    public interface ILogger
    {
        void Debug(string message, string title = null);

        void Info(string message, string title = null);

        void Warning(string message, string title = null);

        void Error(string message, string title = null);

        void Error(Exception exception);

        void Fatal(string message, string title = null);

        void Fatal(Exception exception);
    }

    internal class DefaultLogger : ILogger
    {
        private readonly Type sender;
        private readonly DefaultLoggingService loggingService;

        public DefaultLogger(Type sender, DefaultLoggingService loggingService)
        {
            this.sender = sender;
            this.loggingService = loggingService;
        }

        public void Debug(string message, string title = null)
        {
#if DEBUG
            Log("Debug", title, message);
#endif
        }

        public void Info(string message, string title = null)
        {
            Log("Info", title, message);
        }

        public void Warning(string message, string title = null)
        {
            Log("Warning", title, message);
        }

        public void Error(string message, string title = null)
        {
            Log("Error", title, message);
        }

        public void Error(Exception exception)
        {
            Error(exception.Message, exception.ToString());
        }

        public void Fatal(string message, string title = null)
        {
            Log("Fatal", title, message);
        }

        public void Fatal(Exception exception)
        {
            Fatal(exception.Message, exception.ToString());
        }

        private void Log(string level, string title, string message)
        {
            loggingService.Log(new Log(null, sender.Name, level, title, message, DateTime.Now));
        }
    }
}