using SLStudio.Logging;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SLStudio
{
    internal class AssemblyFinder : IAssemblyFinder
    {
        private static readonly ILogger logger = LogManager.GetLogger<AssemblyFinder>();

        private IEnumerable<string> assembliesCache;

        public IEnumerable<string> FindAssemblies()
        {
            if (assembliesCache != null)
                return assembliesCache;

            logger.Debug("Searching assemblies..");

            var location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var files = Directory.GetFiles(location, "SLStudio.*.dll");

            logger.Debug($"{files.Length} assemblies found");

            assembliesCache = files;
            return files;
        }
    }
}