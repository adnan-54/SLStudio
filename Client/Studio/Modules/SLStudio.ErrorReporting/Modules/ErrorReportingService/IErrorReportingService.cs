using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.ErrorReporting
{
    public interface IErrorReportingService
    {
        void ShowExceptionBox(Exception exception, bool isTerminating);

        void ShowReportDialog(Exception exception);

        Task ReportException(Exception exception, CancellationToken cancellationToken = default);
    }
}