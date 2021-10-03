using System.Collections.Generic;

namespace SLStudio.Web.Api
{
    public class HttpHeaders
    {
        private readonly Dictionary<string, IEnumerable<string>> headers;

        public HttpHeaders()
        {
            headers = new Dictionary<string, IEnumerable<string>>();
            AddHeader("clientId", SharedConstants.ProductId.ToString());
        }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers => headers;

        public void AddHeader(string key, params string[] values)
        {
            headers.TryAdd(key, values);
        }

        public void RemoveHeader(string key)
        {
            headers.Remove(key);
        }
    }
}