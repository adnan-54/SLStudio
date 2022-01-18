using System.Reflection;

namespace SLStudio;

public interface IAssemblyLoader
{
    IEnumerable<Assembly> LoadedAssemblies { get; }

    void LoadAssemblies();
}
