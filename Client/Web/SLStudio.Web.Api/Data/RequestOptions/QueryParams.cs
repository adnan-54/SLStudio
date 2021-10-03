using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Web.Api
{
    public class QueryParams
    {
        private readonly Dictionary<string, string> queryParams;

        public QueryParams()
        {
            queryParams = new Dictionary<string, string>();
        }

        public void AddParam(string key, object value)
        {
            queryParams.TryAdd(key, value?.ToString());
        }

        public void RemoveParam(string key)
        {
            queryParams.Remove(key);
        }

        public string BuildString(Pagination pagination)
        {
            var parameters = queryParams.Union(pagination.GetPagination())
                                          .Where(kvp => kvp.Value != null)
                                          .Select(kvp => $"{kvp.Key}={kvp.Value}");

            if (!parameters.Any())
                return string.Empty;

            return $"?{string.Join("&", parameters)}";
        }
    }
}