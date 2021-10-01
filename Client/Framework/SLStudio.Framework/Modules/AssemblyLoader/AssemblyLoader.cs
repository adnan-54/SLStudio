using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SLStudio
{
    internal class AssemblyLoader : StudioService, IAssemblyLoader
    {
        private const string DLL_SEARCH_PATTERN = "SLStudio.*.dll";

        private static readonly ILogger logger = LogManager.GetLoggerFor<AssemblyLoader>();

        private readonly ISplashScreen splashScreen;

        public AssemblyLoader(ISplashScreen splashScreen)
        {
            this.splashScreen = splashScreen;
        }

        public IEnumerable<Assembly> LoadedAssemblies { get; private set; }

        public void LoadAssemblies()
        {
            if (LoadedAssemblies != null)
                return;

            logger.Debug("Loading assemblies...");
            splashScreen.UpdateStatus("Loading assemblies...");

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();

            logger.Debug($"{loadedAssemblies.Count} assemblies already loaded.");
            splashScreen.UpdateStatus("{0} assemblies already loaded.", loadedAssemblies.Count);

            var dlls = FindDlls().Except(loadedAssemblies.Select(a => a.Location));
            LoadedAssemblies = LoadAssemblies(dlls).Concat(loadedAssemblies);
        }

        private IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> dlls)
        {
            return dlls.Select(LoadAssembly);
        }

        private Assembly LoadAssembly(string file)
        {
            logger.Debug($"Loading assemby {file}...");
            splashScreen.UpdateStatus("Loading assembly {0}...", file);

            return Assembly.LoadFrom(file);
        }

        private IEnumerable<string> FindDlls()
        {
            logger.Debug("Searching dlls...");
            splashScreen.UpdateStatus("Searching dlls...");

            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var files = Directory.GetFiles(directory, DLL_SEARCH_PATTERN);

            logger.Debug($"{files.Length} dlls found.");
            splashScreen.UpdateStatus("{0} dlls found.", files.Length);

            return files;
        }
    }
}