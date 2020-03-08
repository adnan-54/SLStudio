using SLStudio.Core.Utilities.ModuleBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core.Services.BootstrapperService
{
    public interface IBootstrapperService
    {
        IEnumerable<IModule> GetModules();

        Task Initialize();
    }
}