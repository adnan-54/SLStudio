﻿using SLStudio.Core.Events;
using System.Data;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface ILoggingService
    {
        Task Log(NewLogRequestedEvent log);
        DataTable GetLogs();
        void ExportLogsToHtml(string directory);
        void ClearAllLogs();
    }
}
