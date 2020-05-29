using SLStudio.Core.Utilities.Logger;

namespace SLStudio.Core.Events
{
    public class LogCompletedEvent
    {
        public LogCompletedEvent(Log log)
        {
            Log = log;
        }

        public Log Log { get; }
    }
}