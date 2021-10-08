using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Web.Api
{
    internal class RequestManager : IRequestManager
    {
        private readonly IRequestFactory requestFactory;
        private readonly HttpClient client;

        public RequestManager(IRequestFactory requestFactory)
        {
            this.requestFactory = requestFactory;
            client = new HttpClient();
        }

        public Task<ResponseResult> Get(RequestOptions options)
        {
            return MakeRequest(options, HttpMethod.Get);
        }

        public Task<ResponseResult> Post(RequestOptions options)
        {
            return MakeRequest(options, HttpMethod.Post);
        }

        public Task<ResponseResult> Put(RequestOptions options)
        {
            return MakeRequest(options, HttpMethod.Put);
        }

        public Task<ResponseResult> Delete(RequestOptions options)
        {
            return MakeRequest(options, HttpMethod.Delete);
        }

        public Task<ResponseResult> Patch(RequestOptions options)
        {
            return MakeRequest(options, HttpMethod.Patch);
        }

        private async Task<ResponseResult> MakeRequest(RequestOptions options, HttpMethod method)
        {
            using var timeoutCts = new CancellationTokenSource(options.Timeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(options.CancellationToken, timeoutCts.Token);

            var cancelationToken = linkedCts.Token;
            var timeoutCancelationToken = timeoutCts.Token;
            var userCancelationToken = options.CancellationToken;

            try
            {
                var request = requestFactory.CreateRequest(options, method);
                var response = await client.SendAsync(request.RequestMessage, cancelationToken);

                return await ResponseResult.CreateFromResponse(response, cancelationToken);
            }
            catch (Exception ex)
            {
                if (timeoutCancelationToken.IsCancellationRequested)
                    throw new TimeoutException("Request timeout exceeded", ex);
                if (userCancelationToken.IsCancellationRequested)
                    throw new OperationCanceledException("Request canceled", ex);

                throw;
            }
        }
    }
}