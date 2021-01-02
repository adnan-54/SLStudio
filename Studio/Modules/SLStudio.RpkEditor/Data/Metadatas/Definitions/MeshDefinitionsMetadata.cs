using SLStudio.RpkEditor.Modules.Editors.ViewModels;
using SLStudio.RpkEditor.Resources;
using System.Collections.Generic;

namespace SLStudio.RpkEditor.Data
{
    internal class MeshDefinitionsMetadata : ResourceMetadata
    {
        public MeshDefinitionsMetadata()
        {
            DefinitionsEditor = new MeshDefinitionsViewModel(this);
        }

        public override string DisplayName => CommonResources.Mesh;

        public override string Category => CommonResources.Mesh;

        public override IDefinitionsEditor DefinitionsEditor { get; }

        public override int AdditionalType => 5;

        public override ResourceType TypeOfEntry => ResourceType.MeshType;

        public string SourceFile
        {
            get => GetProperty(() => SourceFile);
            set => SetProperty(() => SourceFile, value);
        }

        protected override IEnumerable<ResourceDescription> BuildDescription()
        {
            yield return new ResourceDescription("Loads the file");
            yield return new ResourceDescription($"{SourceFile}", true);
            yield return new ResourceDescription("as a mesh resource, and assing the ID");
            yield return new ResourceDescription($"{TypeId.ToStringId()}", true);
            yield return new ResourceDescription("to it.");
        }
    }
}