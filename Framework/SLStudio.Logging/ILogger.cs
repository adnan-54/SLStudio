using System;

namespace SLStudio.Logging
{
    public interface ILogger
    {
        string Name { get; }

        void Log(string message, string title = null);

        void Log(Exception exception);

        void Debug(string message, string title = null);

        void Debug(Exception exception);

        void Info(string message, string title = null);

        void Info(Exception exception);

        void Warn(string message, string title = null);

        void Warn(Exception exception);

        void Error(string message, string title = null);

        void Error(Exception exception);

        void Fatal(string message, string title = null);

        void Fatal(Exception exception);
    }
}