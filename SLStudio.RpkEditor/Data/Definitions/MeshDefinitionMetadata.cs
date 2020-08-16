using SLStudio.RpkEditor.Editors;
using SLStudio.RpkEditor.Modules.Editors.ViewModels;
using SLStudio.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Data
{
    internal class MeshDefinitionMetadata : ResourceMetadata
    {
        public override int AdditionalType => 5;

        public override ResourceType TypeOfEntry => ResourceType.MeshType;

        public override string DisplayName => CommonResources.Mesh;

        public override string IconSource => "Cube";

        public override string Category => CommonResources.Mesh;

        public override IResourceEditor Editor => new MeshDefinitionsViewModel(this);

        public string SourceFile
        {
            get => GetProperty(() => SourceFile);
            set => SetProperty(() => SourceFile, value);
        }
    }
}