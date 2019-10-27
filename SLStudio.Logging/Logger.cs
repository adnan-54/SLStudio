using System;

namespace SLStudio.Logging
{
    internal class Logger : ILog
    {
        readonly InternalLogManager internalLog = InternalLogManager.GetInstance();

        public string Name { get; private set; }

        public Logger(string name)
        {
            this.Name = name;
        }

        public void Debug(string message = "", string title = "")
        {
            internalLog.InsertIntoLog("Debug", title, message, this.Name, DateTime.Now);
        }

        public void Debug(Exception ex)
        {
            internalLog.InsertIntoLog("Debug", ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }

        public void Error(string message = "", string title = "")
        {
            internalLog.InsertIntoLog("Error", title, message, this.Name, DateTime.Now);
        }

        public void Error(Exception ex)
        {
            internalLog.InsertIntoLog("Error", ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }

        public void Info(string message = "", string title = "")
        {
            internalLog.InsertIntoLog("Info", title, message, this.Name, DateTime.Now);
        }

        public void Info(Exception ex)
        {
            internalLog.InsertIntoLog("Info", ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }

        public void Warning(string message = "", string title = "")
        {
            internalLog.InsertIntoLog("Warning", title, message, this.Name, DateTime.Now);
        }

        public void Warning(Exception ex)
        {
            internalLog.InsertIntoLog("Warning", ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }
    }
}
