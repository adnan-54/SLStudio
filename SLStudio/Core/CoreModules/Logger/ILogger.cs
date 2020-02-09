using System;

namespace SLStudio.Core
{
    public interface ILogger
    {
        void Debug(string message, string title = null);
        void Info(string message, string title = null);
        void Warning(string message, string title = null);
        void Error(string message, string title = null);
        void Error(Exception exception);
        void Fatal(Exception exception);
    }
}
