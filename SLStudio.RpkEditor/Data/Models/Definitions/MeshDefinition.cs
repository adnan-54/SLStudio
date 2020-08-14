using SLStudio.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Rpk.Definitions
{
    internal class MeshDefinition : ResourceMetadata
    {
        public override ResourceType TypeOfEntry => ResourceType.MeshType;

        public override string DisplayName => CommonResources.Mesh;

        public override string IconSource => "Cube";

        public override string Category => CommonResources.Mesh;
    }
}