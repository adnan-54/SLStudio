namespace SlrrLib.Geom
{
    public class NamedScxModel : NamedModel
    {
        public SlrrLib.Model.DynamicScxV3 Scxv3Source = null;
        public SlrrLib.Model.DynamicScxV4 Scxv4Source = null;
        public int MeshIndex = 0;
        public string ScxFnam = "";
        public bool HasPaintableTexture = false;
        public int PaintableTextureUVIndex = -1;

        public NamedScxModel()
        {
        }

        public NamedScxModel(NamedScxModel other)
        {
            Scxv3Source = other.Scxv3Source;
            Scxv4Source = other.Scxv4Source;
            MeshIndex = other.MeshIndex;
            ScxFnam = other.ScxFnam;
            HasPaintableTexture = other.HasPaintableTexture;
            PaintableTextureUVIndex = other.PaintableTextureUVIndex;
        }
    }
}