using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core.CoreModules.Bootstrapper
{
    public interface IBootstrapperService
    {
        IList<IModule> Modules { get; }

        Task Initialize();
    }
}