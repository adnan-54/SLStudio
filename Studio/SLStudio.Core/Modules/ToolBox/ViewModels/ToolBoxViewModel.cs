using SLStudio.Core.Resources;

namespace SLStudio.Core.Modules.ToolBox.ViewModels
{
    internal class ToolBoxViewModel : ToolBase, IToolbox
    {
        public ToolBoxViewModel(IToolManager manager)
        {
            DisplayName = StudioResources.toolbox_displayName;

            manager.Register<IToolbox, ToolBoxViewModel>(this);
        }

        public override WorkspaceItemPlacement Placement => WorkspaceItemPlacement.Left;
    }
}