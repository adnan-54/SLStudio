using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IModuleLoader
    {
        IEnumerable<IModule> Modules { get; }

        void Initialize(ISplashScreen splashScreen, Container container, IObjectFactory objectFactory);

        Task Load();
    }
}