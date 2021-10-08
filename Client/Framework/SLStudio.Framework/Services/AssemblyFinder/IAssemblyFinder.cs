using System.Collections.Generic;

namespace SLStudio
{
    public interface IAssemblyFinder
    {
        IEnumerable<string> FindAssemblies();
    }
}