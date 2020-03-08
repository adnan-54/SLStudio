using System;
using System.Windows;

namespace SLStudio.Core.Utilities.Logger
{
    internal class DefaultLogger : ILogger
    {
        private readonly Type sender;
        private readonly ILoggingService loggingService;
        private readonly ICommandLineArguments commandLineArguments;

        public DefaultLogger(Type sender, ILoggingService loggingService, ICommandLineArguments commandLineArguments)
        {
            this.sender = sender;
            this.loggingService = loggingService;
            this.commandLineArguments = commandLineArguments;
        }


        public void Debug(string message, string title = null)
        {
            if (commandLineArguments.DebuggingMode)
                Log("Debug", title, message);
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
            MessageBox.Show(message, $"Fatal: {title}", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Fatal(Exception exception)
        {
            Fatal(exception.Message, exception.ToString());
        }

        private async void Log(string level, string title, string message)
        {
            await loggingService.Log(sender.Name, level, title, message, DateTime.Now);
        }
    }
}