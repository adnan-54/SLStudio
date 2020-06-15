using System;

namespace SLStudio.Core.Logging
{
    public class LogCompletedEventArgs : EventArgs
    {
        public LogCompletedEventArgs(Log log)
        {
            Log = log;
        }

        public Log Log { get; }
    }
}