using SLStudio.Core;
using SLStudio.RpkEditor.Rpk.Definitions;

namespace SLStudio.RpkEditor.Editors.MeshEditor.ViewModels
{
    internal class MeshEditorViewModel : WindowViewModel
    {
        private readonly MeshDefinition definition;

        public MeshEditorViewModel(MeshDefinition definition)
        {
            this.definition = definition;
        }
    }
}