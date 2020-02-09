using SLStudio.Core.Events;
using System.Data;

namespace SLStudio.Core
{
    public interface ILoggingService
    {
        void Log(NewLogRequestedEvent log);
        DataTable GetLogs();
        void ExportLogsToHtml(string directory);
        void ClearAllLogs();
    }
}
