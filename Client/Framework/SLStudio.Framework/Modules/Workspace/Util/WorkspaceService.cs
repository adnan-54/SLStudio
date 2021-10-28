using AvalonDock;
using DevExpress.Mvvm.UI;

namespace SLStudio
{
    internal class WorkspaceService : ServiceBaseGeneric<DockingManager>, IWorkspaceService
    {
        protected override void OnAttached()
        {
            base.OnAttached();
        }
    }

    internal interface IWorkspaceService
    {
    }
}
