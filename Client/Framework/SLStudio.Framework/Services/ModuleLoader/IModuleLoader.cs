using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IModuleLoader
    {
        IEnumerable<IModule> LoadedModules { get; }

        Task LoadModules(IEnumerable<IModule> modules);
    }
}