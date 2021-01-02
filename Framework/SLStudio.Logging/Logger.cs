using System;

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
            Log(title, message, LogLevel.Debug);
        }

        public void Debug(Exception exception)
        {
            Log(exception, LogLevel.Debug);
        }

        public void Error(string message, string title = null)
        {
            Log(title, message, LogLevel.Error);
        }

        public void Error(Exception exception)
        {
            Log(exception, LogLevel.Error);
        }

        public void Fatal(string message, string title = null)
        {
            Log(title, message, LogLevel.Fatal);
        }

        public void Fatal(Exception exception)
        {
            Log(exception, LogLevel.Fatal);
        }

        public void Info(string message, string title = null)
        {
            Log(title, message, LogLevel.Info);
        }

        public void Info(Exception exception)
        {
            Log(exception, LogLevel.Info);
        }

        public void Log(string message, string title = null)
        {
            Log(title, message, LogManager.Configuraion.DefaultLogLevel);
        }

        public void Log(Exception exception)
        {
            Log(exception, LogManager.Configuraion.DefaultLogLevel);
        }

        public void Warn(string message, string title = null)
        {
            Log(title, message, LogLevel.Warning);
        }

        public void Warn(Exception exception)
        {
            Log(exception, LogLevel.Warning);
        }

        private async void Log(string title, string message, LogLevel level)
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

        private void Log(Exception exception, LogLevel level)
        {
            Log(exception.Message, exception.ToString(), level);
        }
    }
}