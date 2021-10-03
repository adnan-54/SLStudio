using ServiceStack.Text;

namespace SLStudio.Web.Api
{
    public abstract class GenericApiResponse<T> where T : class
    {
        public bool Success { get; init; }

        public string Status { get; init; }

        public string Message { get; init; }

        public T Content { get; init; }

        public override string ToString()
        {
            return $"{{\"{nameof(Success)}\": {Success}, \"{nameof(Status)}\": \"{Status}\", \"{nameof(Message)}\": \"{Message}\", \"{nameof(Content)}\": \"{Content}\"}}";
        }
    }

    public class ApiResponse : GenericApiResponse<string>
    {
        private ApiResponse()
        {
        }

        internal static ApiResponse FromResult(ResponseResult result)
        {
            return new ApiResponse()
            {
                Success = result.Success,
                Status = result.Status,
                Message = result.Message,
                Content = result.Content
            };
        }
    }

    public class ApiResponse<T> : GenericApiResponse<T> where T : class
    {
        private ApiResponse()
        {
        }

        internal static ApiResponse<TContent> FromResult<TContent>(ResponseResult result) where TContent : class
        {
            TContent content = null;

            if (result.Success)
                content = JsonSerializer.DeserializeFromString<TContent>(result.Content);

            return new ApiResponse<TContent>()
            {
                Success = result.Success,
                Status = result.Status,
                Message = result.Message,
                Content = content
            };
        }
    }
}