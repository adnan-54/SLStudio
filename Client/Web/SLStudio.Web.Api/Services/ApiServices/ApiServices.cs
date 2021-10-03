namespace SLStudio.Web.Api
{
    internal class ApiServices : ServiceCollection
    {
        protected override void RegisterServices()
        {
            var requestFactory = new RequestFactory() as IRequestFactory;
            var requestManager = new RequestManager(requestFactory) as IRequestManager;
            var reportExceptionApi = new ReportExceptionApi(requestManager) as IReportExceptionApi;

            RegisterService(requestFactory);
            RegisterService(requestManager);
            RegisterService(reportExceptionApi);
        }
    }
}
