using SLStudio.Core.Docking;

namespace SLStudio.Core.Modules.ToolBox.ViewModels
{
    internal class ToolBoxViewModel : ToolBase
    {
        public ToolBoxViewModel()
        {
            IsVisible = true;
            DisplayName = "Toolbox";
        }

        public override PaneLocation PreferredLocation => PaneLocation.Left;
    }
}