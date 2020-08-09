using SLStudio.RpkEditor.Modules.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkDesignerViewModel : RpkEditorBase
    {
        public RpkDesignerViewModel()
        {
            DisplayName = RpkEditorResources.Design;
            IconSource = "Cube";
        }
    }
}