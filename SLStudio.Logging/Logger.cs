using System;

namespace SLStudio.Logging
{
    public class Logger : ILog
    {
        InternalLogManager internalLog = InternalLogManager.GetInstance();

        public string Name { get; private set; }

        public Logger(string name)
        {
            this.Name = name;
        }

        public void Debug(string message, string title)
        {
            internalLog.InsertIntoLog(LogTypes.Debug.ToString(), title, message, this.Name, DateTime.Now);
        }

        public void Debug(Exception ex)
        {
            internalLog.InsertIntoLog(LogTypes.Debug.ToString(), ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }

        public void Error(string message, string title)
        {
            internalLog.InsertIntoLog(LogTypes.Error.ToString(), title, message, this.Name, DateTime.Now);
        }

        public void Error(Exception ex)
        {
            internalLog.InsertIntoLog(LogTypes.Error.ToString(), ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }

        public void Info(string message, string title)
        {
            internalLog.InsertIntoLog(LogTypes.Info.ToString(), title, message, this.Name, DateTime.Now);
        }

        public void Info(Exception ex)
        {
            internalLog.InsertIntoLog(LogTypes.Info.ToString(), ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }

        public void Warning(string message, string title)
        {
            internalLog.InsertIntoLog(LogTypes.Warning.ToString(), title, message, this.Name, DateTime.Now);
        }

        public void Warning(Exception ex)
        {
            internalLog.InsertIntoLog(LogTypes.Warning.ToString(), ex.ToString(), ex.Message, this.Name, DateTime.Now);
        }
    }
}
