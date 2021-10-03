using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class ModuleFinder : Service, IModuleFinder
    {
        private static readonly ILogger logger = LogManager.GetLogger<ModuleFinder>();

        private IEnumerable<IModule> modulesCache;

        public IEnumerable<IModule> FindModules()
        {
            if (modulesCache != null)
                return modulesCache;

            logger.Debug("Searching modules...");

            var domain = AppDomain.CurrentDomain;
            var types = domain.GetAssemblies()
                              .SelectMany(a => a.GetTypes())
                              .Where(t => t.IsClass)
                              .Where(t => !t.IsAbstract)
                              .Where(t => t.IsAssignableTo(typeof(Module)))
                              .ToList();

            logger.Debug($"{types.Count} modules found");

            var modules = types.Select(t => Activator.CreateInstance(t) as IModule)
                               .OrderByDescending(m => m.Priority);
            modulesCache = modules;

            return modules;
        }
    }
}