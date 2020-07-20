using SLStudio.Core.Resources.Language;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultAssemblyLoader : IAssemblyLoader
    {
        private const string SEARCH_PATTERN = "SLStudio.*.dll";

        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultAssemblyLoader>();

        private ISplashScreen splashScreen;
        private bool alreadyInitialized = false;
        private bool alreadyLoaded = false;

        public IEnumerable<string> Assemblies { get; private set; }

        public IEnumerable<string> LoadedAssemblies { get; private set; }

        public IEnumerable<string> AssembliesToLoad { get; private set; }

        public void Initialize(ISplashScreen splashScreen)
        {
            if (alreadyInitialized)
                return;
            alreadyInitialized = true;
            this.splashScreen = splashScreen;
        }

        public async Task Load()
        {
            if (!alreadyInitialized)
                throw new InvalidOperationException($"{nameof(DefaultAssemblyLoader)} is not initialized yet");

            if (alreadyLoaded)
                return;
            alreadyLoaded = true;

            logger.Debug($"Searching assemblies");

            await Task.Run(() =>
            {
                Assemblies = FindAssemblies();
                LoadedAssemblies = GetLoadedAssemblies();
                AssembliesToLoad = Assemblies.Except(LoadedAssemblies);

                LoadAssemblies(AssembliesToLoad);
            });
        }

        private IEnumerable<string> FindAssemblies()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Directory.GetFiles(directory, SEARCH_PATTERN);
        }

        private IEnumerable<string> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Select(a => a.Location);
        }

        private void LoadAssemblies(IEnumerable<string> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var assemblyName = Path.GetFileNameWithoutExtension(assembly);
                logger.Debug($"Loading assembly '{assemblyName}'");
                splashScreen.UpdateStatus(string.Format(Resources.Language.Language.LoadingAssemblyAssemblyName, assemblyName));
                Assembly.LoadFrom(assembly);
            }
        }
    }
}