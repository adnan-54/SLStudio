using ServiceStack.Text;
using System;
using System.Net.Http;
using System.Text;

namespace SLStudio.Web.Api
{
    internal class RequestFactory : IRequestFactory
    {
        public ApiRequest CreateRequest(RequestOptions options, HttpMethod method)
        {
            var url = options.BuildUrl();
            var body = JsonSerializer.SerializeToString(options.Content);
            var message = CreateRequestMessage(options, method, url, body);

            return new ApiRequest()
            {
                Url = url,
                Content = body,
                RequestMessage = message
            };
        }

        private static HttpRequestMessage CreateRequestMessage(RequestOptions options, HttpMethod method, string url, string content)
        {
            var requestMessage = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            AddRequestHeaders(options, requestMessage);

            return requestMessage;
        }

        private static void AddRequestHeaders(RequestOptions options, HttpRequestMessage requestMessage)
        {
            var requestHeaders = requestMessage.Headers;
            var language = options.Culture.Name;
            var headers = options.Headers.Headers;

            foreach (var header in headers)
                requestHeaders.Add(header.Key, header.Value);

            requestHeaders.AcceptLanguage.Add(new(language));
            requestHeaders.Date = DateTime.Now;

            //todo: add token when user autentication service is ready
            if (options.Authenticate)
                requestHeaders.Authorization = new("Bearer", "invalidtoken");
        }
    }
}