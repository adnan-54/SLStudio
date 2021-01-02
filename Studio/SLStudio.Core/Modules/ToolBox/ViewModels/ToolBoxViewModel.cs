using SLStudio.Core.Resources;

namespace SLStudio.Core.Modules.ToolBox.ViewModels
{
    internal class ToolBoxViewModel : ToolPanelBase, IToolbox
    {
        public ToolBoxViewModel(IToolManager manager)
        {
            DisplayName = StudioResources.Toolbox;
            manager.Register<IToolbox, ToolBoxViewModel>(this);
        }

        public override ToolPlacement Placement => ToolPlacement.Left;
    }
}