using DevExpress.Mvvm;
using SLStudio.RpkEditor.Editors;
using SLStudio.RpkEditor.Rpk.Definitions;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class MeshDefinitionsViewModel : ViewModelBase, IResourceEditor
    {
        public MeshDefinitionsViewModel(MeshDefinition definition)
        {
            Definition = definition;
            SourceFile = definition.SourceFile;
        }

        public MeshDefinition Definition { get; }

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