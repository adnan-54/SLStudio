using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IModuleLookup
    {
        IEnumerable<IModule> Modules { get; }

        void Initialize(ISplashScreen splashScreen, Container container, IObjectFactory objectFactory);

        Task Load();
    }
}