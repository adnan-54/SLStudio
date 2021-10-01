using Newtonsoft.Json.Linq;
using SLStudio.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SLStudio.Api
{
    internal class DefaultStudioClient : IStudioClient
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultStudioClient>();
        private static readonly string VersionRoute = "version/";

        private readonly HttpClient client;
        private Version version;

        public DefaultStudioClient()
        {
            client = new HttpClient();
        }

        public async Task<Version> GetVersion()
        {
            if (version is null)
            {
                logger.Debug("fetching current version...");

                using var response = await client.GetAsync($"{StudioConstants.ApiRoute}{VersionRoute}");
                using var content = response.Content;
                var data = await content.ReadAsStringAsync();
                var versionString = JObject.Parse(data)["version"].ToString();
                version = new Version(versionString);
            }

            return version;
        }
    }

    public interface IStudioClient
    {
        Task<Version> GetVersion();
    }
}