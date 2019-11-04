using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Modules.Shell.Views;

namespace SLStudio.Studio.Core.Modules.Shell.Services
{
    public interface ILayoutItemStatePersister
    {
        bool SaveState(IShell shell, IShellView shellView, string fileName);
        bool LoadState(IShell shell, IShellView shellView, string fileName);
    }
}