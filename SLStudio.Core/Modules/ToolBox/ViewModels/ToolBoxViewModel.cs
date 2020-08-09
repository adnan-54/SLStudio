using SLStudio.Core.Resources;

namespace SLStudio.Core.Modules.ToolBox.ViewModels
{
    internal class ToolBoxViewModel : ToolPanelBase, IToolbox
    {
        private readonly IToolboxContent defaultContent;

        public ToolBoxViewModel()
        {
            defaultContent = new DefaultContentViewModel();
            DisplayName = StudioResources.Toolbox;
        }

        public override ToolPlacement Placement => ToolPlacement.Left;

        public IToolboxContent ToolboxContent
        {
            get => GetProperty(() => ToolboxContent);
            set
            {
                if (ToolboxContent == value)
                    return;
                SetProperty(() => ToolboxContent, value);
            }
        }

        public void SetContent(IPanelItem host)
        {
            if (host is IHaveToolbox toolboxHost)
            {
                if (toolboxHost.ToolboxContent != null)
                    ToolboxContent = toolboxHost.ToolboxContent;
                else
                    ToolboxContent = defaultContent;
            }
        }
    }
}