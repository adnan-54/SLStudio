using SLStudio.FileTypes.Attributes;
using SLStudio.GameTypes;

namespace SLStudio.FileTypes.RpkFile
{
    public class LightDefinition : ResourceBase
    {
        public override ResourceType TypeOfEntry => ResourceType.Light;

        public TextureDefinition LensFlare { get; set; }

        public TextureDefinition HorizontalAttenuation { get; set; }

        public TextureDefinition VerticalAttenuation { get; set; }

        [ResourceAttribute("type", 0)]
        public LightSourceType Type { get; set; }

        [ResourceAttribute("diffuse", 1)]
        public ArgbColor Diffuse { get; set; }

        [ResourceAttribute("specular", 2)]
        public ArgbColor Specular { get; set; }

        [ResourceAttribute("ambient", 3)]
        public ArgbColor Ambient { get; set; }

        [ResourceAttribute("position", 4)]
        public Position Position { get; set; }

        [ResourceAttribute("direction", 5)]
        public Rotation Direction { get; set; }

        [ResourceAttribute("range", 6)]
        public double Range { get; set; }

        [ResourceAttribute("attenuation", 7)]
        public Position Attenuation { get; set; }

        [ResourceAttribute("theta", 8)]
        public double Theta { get; set; }

        [ResourceAttribute("phi", 9)]
        public double Phi { get; set; }

        [ResourceAttribute("lf_texture", 10)]
        public int LensFlareTexture => LensFlare.TypeId;

        [ResourceAttribute("lf_glow_color", 11)]
        public ArgbColor LensFlareGlowColor { get; set; }
    }

    public enum LightSourceType
    {
        Spot,
        FakeSpot
    }
}