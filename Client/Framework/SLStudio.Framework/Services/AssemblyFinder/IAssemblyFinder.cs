using System.Collections.Generic;

namespace SLStudio
{
    public interface IAssemblyFinder : IService
    {
        IEnumerable<string> FindAssemblies();
    }
}