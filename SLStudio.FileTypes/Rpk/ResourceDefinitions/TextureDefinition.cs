using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.TextureFile;

namespace SLStudio.FileTypes.RpkFile
{
    public class TextureDefinition : ResourceBase
    {
        public override int AdditionalType => 4;

        public override ResourceType TypeOfEntry => ResourceType.Texture;

        public Texture Texture { get; set; }

        [ResourceAttribute("sourcefile", 0)]
        public string SourceFile => Texture.Path;
    }
}