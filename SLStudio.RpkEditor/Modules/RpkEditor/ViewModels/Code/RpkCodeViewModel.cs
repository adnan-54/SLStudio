using SLStudio.RpkEditor.Modules.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkCodeViewModel : RpkEditorBase
    {
        public RpkCodeViewModel()
        {
            DisplayName = RpkEditorResources.Code;
            IconSource = "CodeDefinition";
        }
    }
}