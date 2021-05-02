using SLStudio.Core.Resources;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultAssemblyLookup : IAssemblyLookup
    {
        private const string SEARCH_PATTERN = "SLStudio.*.dll";

        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultAssemblyLookup>();

        private readonly IList<Assembly> loadedAssemblies = new List<Assembly>();
        private ISplashScreen splashScreen;
        private bool alreadyInitialized = false;
        private bool alreadyLoaded = false;

        public IEnumerable<Assembly> LoadedAssemblies => loadedAssemblies;

        public void Initialize(ISplashScreen splashScreen)
        {
            if (alreadyInitialized)
                return;
            alreadyInitialized = true;
            this.splashScreen = splashScreen;
        }

        public async Task Load()
        {
            await Task.Run(() =>
            {
                if (!alreadyInitialized)
                    throw new InvalidOperationException($"{nameof(DefaultAssemblyLookup)} is not initialized yet");

                if (alreadyLoaded)
                    return;
                alreadyLoaded = true;

                logger.Debug($"Searching assemblies");

                var assemblies = FindAssemblies();
                var loadedAssemblies = GetLoadedAssemblies();
                var assembliesToLoad = assemblies.Except(loadedAssemblies);

                LoadAssemblies(assembliesToLoad);
            });
        }

        private static IEnumerable<string> FindAssemblies()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Directory.GetFiles(directory, SEARCH_PATTERN);
        }

        private static IEnumerable<string> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(a => a.Location);
        }

        private void LoadAssemblies(IEnumerable<string> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var assemblyName = Path.GetFileNameWithoutExtension(assembly);
                logger.Debug($"Loading assembly {assemblyName}");
                splashScreen.UpdateStatus(string.Format(StudioResources.LoadingAssemblyAssemblyName, assemblyName));
                var loaded = Assembly.LoadFrom(assembly);
                loadedAssemblies.Add(loaded);
            }
        }
    }
}