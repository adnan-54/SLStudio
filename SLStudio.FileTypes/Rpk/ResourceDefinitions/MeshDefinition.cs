using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.MeshFile;

namespace SLStudio.FileTypes.RpkFile
{
    public class MeshDefinition : ResourceDefinition
    {
        public override ResourceType TypeOfEntry => ResourceType.Mesh;

        public Mesh Mesh { get; set; }

        [ResourceAttribute("sourcefile", 0)]
        public string SourceFile => Mesh.Path;
    }
}