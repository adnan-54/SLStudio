using System;
using System.Data;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface ILoggingService
    {
        bool LogFileExists { get; }
        bool SimpleLogFileExists { get; }

        Task Log(string sender, string level, string title, string message, DateTime date);

        DataTable GetLogs();

        void ExportLogsToHtml(string directory);

        void ClearAllLogs();

        string GetSimpleLogs();
    }
}