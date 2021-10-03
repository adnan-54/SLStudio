using System.Collections.Generic;

namespace SLStudio
{
    public interface IModuleFinder : IService
    {
        IEnumerable<IModule> FindModules();
    }
}