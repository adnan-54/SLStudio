using Caliburn.Micro;
using SLStudio.Studio.Core.Modules.Output;
using System;

namespace SLStudio.Studio.Core.Modules.Logging
{
    internal class Logger : ILogger
    {
        public Logger(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public void Debug(string title = "", string message = "")
        {
            LogManager.Log(title, message, "Debug", Name);
        }

        public void Info(string title = "", string message = "")
        {
            LogManager.Log(title, message, "Info", Name);
        }

        public void Warning(string title = "", string message = "")
        {
            LogManager.Log(title, message, "Warning", Name);
        }

        public void Error(string title = "", string message = "")
        {
            LogManager.Log(title, message, "Error", Name);
        }

        public void Error(Exception exception)
        {
            Error(exception.Message, exception.ToString());
        }
    }
}
