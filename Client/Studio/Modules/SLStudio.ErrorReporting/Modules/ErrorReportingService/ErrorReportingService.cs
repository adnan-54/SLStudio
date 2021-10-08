using SLStudio.Web.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.ErrorReporting
{
    internal class ErrorReportingService : IErrorReportingService
    {
        private readonly IReportExceptionApi reportExceptionApi;

        public ErrorReportingService(IReportExceptionApi reportExceptionApi)
        {
            this.reportExceptionApi = reportExceptionApi;
        }

        public void ShowExceptionBox(Exception exception, bool isTerminating)
        {
            new ExceptionBox(exception, isTerminating).ShowDialog();
        }

        public void ShowReportDialog(Exception exception)
        {
            new ReportExceptionDialog(this, exception).ShowDialog();
        }

        public Task ReportException(Exception exception, CancellationToken cancellationToken)
        {
            return reportExceptionApi.ReportException(exception, cancellationToken);
        }
    }
}