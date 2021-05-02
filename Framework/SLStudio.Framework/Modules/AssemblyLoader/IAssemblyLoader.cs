using System.Collections.Generic;
using System.Reflection;

namespace SLStudio
{
    public interface IAssemblyLoader : IStudioService
    {
        IEnumerable<Assembly> LoadedAssemblies { get; }

        void LoadAssemblies();
    }
}