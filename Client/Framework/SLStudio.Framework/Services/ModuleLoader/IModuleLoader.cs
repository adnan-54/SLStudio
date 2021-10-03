using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IModuleLoader : IService
    {
        IEnumerable<IModule> LoadedModules { get; }

        Task LoadModules(IEnumerable<IModule> modules);
    }
}