using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IAssemblyLookup
    {
        IEnumerable<Assembly> LoadedAssemblies { get; }

        void Initialize(ISplashScreen splashScreen);

        Task Load();
    }
}