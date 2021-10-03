using SLStudio.Web.Api;

namespace SLStudio.ErrorReporting
{
    internal class ErrorReportingServices : ServiceCollection
    {
        protected override void RegisterServices()
        {
            var reportExceptionApi = WebApiModule.ReportExceptionApi;
            var errorReportingService = new ErrorReportingService(reportExceptionApi) as IErrorReportingService;

            RegisterService(errorReportingService);
        }
    }
}