namespace SLStudio.Rpk
{
    public abstract class ResourceDefinition
    {
        public abstract ResourceType TypeOfEntry { get; }
    }

    public class MeshDefinition : ResourceDefinition
    {
        public override ResourceType TypeOfEntry => ResourceType.Mesh;
    }

    public class SoundDefinition : ResourceDefinition
    {
        public override ResourceType TypeOfEntry => ResourceType.Sound;
    }
}