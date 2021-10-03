using System.Net.Http;

namespace SLStudio.Web.Api
{
    public interface IRequestFactory : IService
    {
        ApiRequest CreateRequest(RequestOptions options, HttpMethod method);
    }
}