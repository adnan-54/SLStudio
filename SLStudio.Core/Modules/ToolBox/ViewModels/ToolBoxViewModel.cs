namespace SLStudio.Core.Modules.ToolBox.ViewModels
{
    internal class ToolBoxViewModel : ToolPanelBase
    {
        public ToolBoxViewModel()
        {
            DisplayName = "Toolbox";
        }

        public override ToolPlacement Placement => ToolPlacement.Left;

        public string TestText
        {
            get => GetProperty(() => TestText);
            set => SetProperty(() => TestText, value);
        }
    }
}