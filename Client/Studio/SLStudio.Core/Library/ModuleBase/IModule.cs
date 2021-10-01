using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IModule
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        string Version { get; }
        int Priority { get; }
        bool ShouldBeLoaded { get; }
        bool Loaded { get; }

        IEnumerable<Uri> Resources { get; }

        void MergeResources(ISplashScreen splashScreen);

        void Register(ISplashScreen splashScreen, Container container);

        Task Run(ISplashScreen splashScreen, IObjectFactory objectFactory);
    }
}