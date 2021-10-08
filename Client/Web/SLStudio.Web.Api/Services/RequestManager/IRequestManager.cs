using System.Threading.Tasks;

namespace SLStudio.Web.Api
{
    public interface IRequestManager
    {
        Task<ResponseResult> Get(RequestOptions options);

        Task<ResponseResult> Post(RequestOptions options);

        Task<ResponseResult> Put(RequestOptions options);

        Task<ResponseResult> Delete(RequestOptions options);

        Task<ResponseResult> Patch(RequestOptions options);
    }
}