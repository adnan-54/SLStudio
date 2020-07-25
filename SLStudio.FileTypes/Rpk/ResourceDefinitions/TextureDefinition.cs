using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.TextureFile;

namespace SLStudio.FileTypes.RpkFile
{
    public class TextureDefinition : ResourceDefinition
    {
        internal override ResourceType TypeOfEntry => ResourceType.Texture;

        internal override int AdditionalType => 4;

        public Texture Texture { get; set; }

        [ResourceAttribute("sourcefile", 0)]
        public string SourceFile => Texture.Path;
    }
}