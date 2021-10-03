using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Web.Api
{
    public class ResponseResult
    {
        private ResponseResult()
        {
        }

        public bool Success { get; init; }

        public string Message { get; init; }

        public string Status { get; init; }

        public string Content { get; init; }

        public static async Task<ResponseResult> CreateFromResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            var success = response.IsSuccessStatusCode;
            var status = response.StatusCode.ToString("d");
            var message = success ? response.ReasonPhrase : $"{status} - {response.ReasonPhrase}: {body}";
            var content = success ? body : string.Empty;

            return new ResponseResult()
            {
                Success = success,
                Message = message,
                Status = status,
                Content = content,
            };
        }
    }
}