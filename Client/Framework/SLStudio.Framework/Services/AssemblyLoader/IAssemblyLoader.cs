using System.Collections.Generic;
using System.Reflection;

namespace SLStudio
{
    public interface IAssemblyLoader : IService
    {
        IEnumerable<Assembly> LoadedAssemblies { get; }

        void LoadAssemblies(IEnumerable<string> assemblies);
    }
}