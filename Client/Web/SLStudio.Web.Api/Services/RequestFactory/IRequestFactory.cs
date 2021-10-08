using System.Net.Http;

namespace SLStudio.Web.Api
{
    public interface IRequestFactory
    {
        ApiRequest CreateRequest(RequestOptions options, HttpMethod method);
    }
}