using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Web.Api
{
    internal class ReportExceptionApi : Service, IReportExceptionApi
    {
        private readonly IRequestManager requestManager;

        public ReportExceptionApi(IRequestManager requestManager)
        {
            this.requestManager = requestManager;
        }

        public async Task<ApiResponse> ReportException(Exception exception, CancellationToken cancellationToken)
        {
            var requestOptions = new RequestOptions()
            {
                Content = new { exception },
                Route = "report-exception",
                CancellationToken = cancellationToken
            };

            var result = await requestManager.Post(requestOptions);

            return ApiResponse.FromResult(result);
        }
    }
}