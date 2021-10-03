using System;
using System.Collections.Generic;
using System.Reflection;
using SLStudio.Logging;

namespace SLStudio
{
    internal class AssemblyLoader : Service, IAssemblyLoader
    {
        private static readonly ILogger logger = LogManager.GetLogger<AssemblyLoader>();

        private List<Assembly> loadedAssemblies;

        public IEnumerable<Assembly> LoadedAssemblies => loadedAssemblies;

        public void LoadAssemblies(IEnumerable<string> assemblies)
        {
            if (loadedAssemblies != null)
                throw new InvalidOperationException("Assemblies already loaded");
            loadedAssemblies = new List<Assembly>();

            logger.Debug("Starting to load assemblies...");

            foreach (var assembly in assemblies)
                LoadAssembly(assembly);

            logger.Debug("Finished to load assemblies...");
        }

        private void LoadAssembly(string assembly)
        {
            try
            {
                logger.Debug($"Loading assembly {assembly}...");

                var assemblyName = AssemblyName.GetAssemblyName(assembly);
                var loaded = AppDomain.CurrentDomain.Load(assemblyName);
                loadedAssemblies.Add(loaded);

                logger.Debug($"Assembly {assembly} loaded successfully");
            }
            catch (Exception ex)
            {
                logger.Debug($"Failed to load assembly {assembly}");
                logger.Error(ex);
            }
        }
    }
}