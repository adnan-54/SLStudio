using System.Net.Http;

namespace SLStudio.Web.Api
{
    public class ApiRequest
    {
        public string Url { get; init; }

        public string Content { get; init; }

        public HttpRequestMessage RequestMessage { get; init; }
    }
}