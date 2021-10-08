using System.Collections.Generic;

namespace SLStudio
{
    public interface IModuleFinder
    {
        IEnumerable<IModule> FindModules();
    }
}