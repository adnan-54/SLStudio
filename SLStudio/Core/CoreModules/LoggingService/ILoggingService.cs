using System.Data;

namespace SLStudio.Core
{
    public interface ILoggingService
    {
        DataTable GetLogs();
        void ExportLogsToHtml(string directory);
        void ClearAllLogs();
    }
}
