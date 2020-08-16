using DevExpress.Mvvm;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Editors;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class MeshDefinitionsViewModel : ViewModelBase, IResourceEditor
    {
        public MeshDefinitionsViewModel(MeshDefinitionMetadata definition)
        {
            Definition = definition;
            SourceFile = definition.SourceFile;
        }

        public MeshDefinitionMetadata Definition { get; }

        public string SourceFile
        {
            get => GetProperty(() => SourceFile);
            set
            {
                SetProperty(() => SourceFile, value);
            }
        }

        public bool IsValid => true;

        public void ApplyChanges()
        {
            Definition.SourceFile = SourceFile;
        }
    }
}