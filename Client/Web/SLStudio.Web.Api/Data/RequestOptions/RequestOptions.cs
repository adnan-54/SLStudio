using System;
using System.Globalization;
using System.Threading;

namespace SLStudio.Web.Api
{
    public class RequestOptions
    {
        public RequestOptions()
        {
            QueryParams = new QueryParams();
            Headers = new HttpHeaders();
            Pagination = new Pagination();
            Timeout = SharedConstants.DefaultTimeout;
            Route = string.Empty;
            Content = new { };
            Authenticate = false;
            Culture = Thread.CurrentThread.CurrentUICulture;
            CancellationToken = CancellationToken.None;
        }

        public QueryParams QueryParams { get; }

        public HttpHeaders Headers { get; }

        public Pagination Pagination { get; init; }

        public TimeSpan Timeout { get; init; }

        public string Route { get; init; }

        public object Content { get; init; }

        public bool Authenticate { get; init; }

        public CultureInfo Culture { get; init; }

        public CancellationToken CancellationToken { get; init; }

        public string BuildUrl()
        {
            return $"{SharedConstants.ApiRoute}/{Route}{QueryParams.BuildString(Pagination)}";
        }
    }
}