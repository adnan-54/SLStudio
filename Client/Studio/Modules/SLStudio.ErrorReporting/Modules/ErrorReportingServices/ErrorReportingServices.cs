using SLStudio.Web.Api;

namespace SLStudio.ErrorReporting
{
    internal class ErrorReportingServices : ServiceContainer
    {
        protected override void Initialize()
        {
            var reportExceptionApi = ApiModule.ServiceContainer.GetService<IReportExceptionApi>();
            var errorReportingService = new ErrorReportingService(reportExceptionApi) as IErrorReportingService;

            RegisterService(errorReportingService);
        }
    }
}