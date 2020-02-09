using Caliburn.Micro;
using SLStudio.Core.Events;
using System;

namespace SLStudio.Core.CoreModules.Logging
{
    internal class Logger : ILogger
    {
        private readonly Type sender;
        private readonly IEventAggregator eventAggregator;

        public Logger(Type sender, IEventAggregator eventAggregator)
        {
            this.sender = sender;
            this.eventAggregator = eventAggregator;
        }

        private enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public void Debug(string message, string title = null)
        {
            Log(LogLevel.Debug.ToString(), title, message);
        }

        public void Info(string message, string title = null)
        {
            Log(LogLevel.Info.ToString(), title, message);
        }

        public void Warning(string message, string title = null)
        {
            Log(LogLevel.Warning.ToString(), title, message);
        }

        public void Error(string message, string title = null)
        {
            Log(LogLevel.Error.ToString(), title, message);
        }

        public void Error(Exception exception)
        {
            Error(exception.Message, exception.ToString());
        }

        private void Log(string level, string title, string description)
        {
            eventAggregator.PublishOnUIThread(new NewLogRequestedEvent(sender.Name, level, title, description, DateTime.Now));
        }
    }
}
