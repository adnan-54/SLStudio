using System;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    internal class Logger : ILogger
    {
        private readonly LoggingService loggingService;

        public Logger(string name, LoggingService loggingService)
        {
            Name = name;
            this.loggingService = loggingService;
        }

        public string Name { get; }

        public void Debug(string message, string title = null)
        {
            _ = Log(title, message, LogLevel.Debug);
        }

        public void Debug(Exception exception)
        {
            LogException(exception, LogLevel.Debug);
        }

        public void Error(string message, string title = null)
        {
            _ = Log(title, message, LogLevel.Error);
        }

        public void Error(Exception exception)
        {
            LogException(exception, LogLevel.Error);
        }

        public void Fatal(string message, string title = null)
        {
            _ = Log(title, message, LogLevel.Fatal);
        }

        public void Fatal(Exception exception)
        {
            LogException(exception, LogLevel.Fatal);
        }

        public void Info(string message, string title = null)
        {
            _ = Log(title, message, LogLevel.Info);
        }

        public void Info(Exception exception)
        {
            LogException(exception, LogLevel.Info);
        }

        public void Log(string message, string title = null)
        {
            _ = Log(title, message, LogManager.Configuraion.DefaultLogLevel);
        }

        public void Log(Exception exception)
        {
            LogException(exception, LogManager.Configuraion.DefaultLogLevel);
        }

        public void Warn(string message, string title = null)
        {
            _ = Log(title, message, LogLevel.Warning);
        }

        public void Warn(Exception exception)
        {
            LogException(exception, LogLevel.Warning);
        }

        private void LogException(Exception exception, LogLevel level)
        {
            _ = Log(exception.Message, exception.ToString(), level);
        }

        private async Task Log(string title, string message, LogLevel level)
        {
            if (level == LogLevel.Debug && LogManager.Configuraion.IgnoreDebugLevel)
                return;

            var result = await loggingService.CreateLog(title, message, Name, level);
            if (result != null)
            {
                if (LogManager.Configuraion.LogToConsole)
                {
                    Console.WriteLine($"{result}");
                    System.Diagnostics.Debug.WriteLine($"{result}");
                }

                LogManager.OnLogCompleted(result);
            }
        }
    }
}