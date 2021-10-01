using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IModuleLoader : IStudioService
    {
        IEnumerable<IModuleInfo> LoadedModules { get; }

        Task LoadModules();
    }
}