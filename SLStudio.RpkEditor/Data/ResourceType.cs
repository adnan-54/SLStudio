using SLStudio.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Data
{
    public class ResourceType
    {
        private ResourceType(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }

        public int Id { get; }

        public static ResourceType MeshType => new ResourceType(CommonResources.Mesh, 5);
    }
}