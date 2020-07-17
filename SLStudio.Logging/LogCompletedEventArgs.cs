using System;

namespace SLStudio.Logging
{
    public class LogCompletedEventArgs : EventArgs
    {
        internal LogCompletedEventArgs()
        {
        }

        internal LogCompletedEventArgs(Log log)
        {
            Log = log;
        }

        public Log Log { get; }
    }
}