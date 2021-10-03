using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Web.Api
{
    public interface IReportExceptionApi : IService
    {
        Task<ApiResponse> ReportException(Exception exception, CancellationToken cancellationToken = default);
    }
}