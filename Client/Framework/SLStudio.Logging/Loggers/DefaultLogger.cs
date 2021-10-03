using System;

namespace SLStudio.Logging
{
    internal class DefaultLogger : ILogger
    {
        private readonly ILoggingService loggingService;

        public DefaultLogger(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public string Name { get; set; }

        public void Log(object message, string title, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(title, message, null, callerFile, callerMember, callerLine);
        }

        public void Log(Exception exception, string callerFile, string callerMember, int callerLine)
        {
            LogException(exception, null, callerFile, callerMember, callerLine);
        }

        public void Debug(object message, string title, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(title, message, $"{LogLevel.Debug}", callerFile, callerMember, callerLine);
        }

        public void Debug(Exception exception, string callerFile, string callerMember, int callerLine)
        {
            LogException(exception, $"{LogLevel.Debug}", callerFile, callerMember, callerLine);
        }

        public void Info(object message, string title, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(title, message, $"{LogLevel.Info}", callerFile, callerMember, callerLine);
        }

        public void Info(Exception exception, string callerFile, string callerMember, int callerLine)
        {
            LogException(exception, $"{LogLevel.Info}", callerFile, callerMember, callerLine);
        }

        public void Warn(object message, string title, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(title, message, $"{LogLevel.Warning}", callerFile, callerMember, callerLine);
        }

        public void Warn(Exception exception, string callerFile, string callerMember, int callerLine)
        {
            LogException(exception, $"{LogLevel.Warning}", callerFile, callerMember, callerLine);
        }

        public void Error(object message, string title, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(title, message, $"{LogLevel.Error}", callerFile, callerMember, callerLine);
        }

        public void Error(Exception exception, string callerFile, string callerMember, int callerLine)
        {
            LogException(exception, $"{LogLevel.Error}", callerFile, callerMember, callerLine);
        }

        public void Fatal(object message, string title, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(title, message, $"{LogLevel.Fatal}", callerFile, callerMember, callerLine);
        }

        public void Fatal(Exception exception, string callerFile, string callerMember, int callerLine)
        {
            LogException(exception, $"{LogLevel.Fatal}", callerFile, callerMember, callerLine);
        }

        private void LogException(Exception exception, string level, string callerFile, string callerMember, int callerLine)
        {
            Enqueue(exception.Message, exception, level, callerFile, callerMember, callerLine);
        }

        private void Enqueue(string title, object message, string level, string callerFile, string callerMember, int callerLine)
        {
            loggingService.EnqueueLog(Name, title, $"{message}", level, callerFile, callerMember, callerLine);
        }
    }
}