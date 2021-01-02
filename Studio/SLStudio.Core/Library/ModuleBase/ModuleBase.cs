using SimpleInjector;
using SLStudio.Core.Resources;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core
{
    public abstract class ModuleBase : IModule
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<ModuleBase>();

        private bool alreadyMerged = false;
        private bool alreadyRegistered = false;
        private bool alreadyRunned = false;

        public virtual string Name => $"{GetType().Assembly.GetName().Name}";

        public virtual string Description => $"{GetType().Assembly.GetName().FullName}";

        public virtual string Author => $"{FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).CompanyName}";

        public virtual string Version => $"{GetType().Assembly.GetName().Version}";

        public virtual int Priority => 50;

        public virtual bool ShouldBeLoaded => true;

        public bool Loaded => alreadyMerged && alreadyRegistered && alreadyRunned;

        public virtual IEnumerable<Uri> Resources
            => Enumerable.Empty<Uri>();

        public void MergeResources(ISplashScreen splashScreen)
        {
            if (alreadyMerged)
                return;
            alreadyMerged = true;

            logger.Debug($"Merging resources for '{Name}'");
            splashScreen.UpdateStatus(string.Format(StudioResources.MergingResources, Name));
            foreach (var resource in Resources)
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = resource });
        }

        public void Register(ISplashScreen splashScreen, Container container)
        {
            if (alreadyRegistered)
                return;
            alreadyRegistered = true;

            logger.Debug($"Registering '{Name}'");
            splashScreen.UpdateStatus(string.Format(StudioResources.RegisteringModule, Name));

            Register(container);
        }

        protected virtual void Register(Container container)
        {
            return;
        }

        public Task Run(ISplashScreen splashScreen, IObjectFactory objectFactory)
        {
            if (alreadyRunned)
                return Task.FromResult(true);
            alreadyRunned = true;

            logger.Debug($"Running '{Name}'");
            splashScreen.UpdateStatus(string.Format(StudioResources.RunningModule, Name));

            return Run(objectFactory);
        }

        protected virtual Task Run(IObjectFactory objectFactory)
        {
            return Task.FromResult(true);
        }
    }
}