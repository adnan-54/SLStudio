using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SLStudio.Logger
{
    public class Logger: ILogger
    {
        private readonly string logPath = Path.Combine(Application.StartupPath, "logger");

        private readonly string owner;
        private bool isInitialized = false;

        public Logger(string owner)
        {
            this.owner = owner;

            Initialize();
        }

        public void Error(Exception exception)
        {
            Log($"Message: {exception.Message}\nStackTrace: {exception.StackTrace}\nSource: {exception.Source}", exception.ToString(), LogType.Error);
        }

        public void Error(string message, string title)
        {
            Log(message, message, LogType.Error);
        }

        public void Info(string message, string title)
        {
            Log(message, message, LogType.Info);
        }

        public void Warning(string message, string title)
        {
            Log(message, message, LogType.Warning);
        }

        public DataTable GetLog()
        {
            throw new NotImplementedException();
        }

        private void Log(string message, string title, LogType type)
        {
            
        }

        private void Initialize()
        {
            

            isInitialized = true;
        }
    }

    enum LogType
    {
        Error,
        Info,
        Warning
    }
}
