using Caliburn.Micro;
using SLStudio.Core.Events;
using System;
using System.Windows;

namespace SLStudio.Core.CoreModules.Logging
{
    internal class Logger : ILogger
    {
        private readonly Type sender;
        private readonly ILoggingService loggingService;

        public Logger(Type sender)
        {
            this.sender = sender;
            loggingService = IoC.Get<ILoggingService>();
        }

        private enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal
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

        public void Fatal(Exception exception)
        {
            Log(LogLevel.Fatal.ToString(), exception.Message, exception.ToString());
            MessageBox.Show(exception.ToString(), "Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void Log(string level, string title, string description)
        {
            await loggingService.Log(new NewLogRequestedEvent(sender.Name, level, title, description, DateTime.Now));
        }
    }
}