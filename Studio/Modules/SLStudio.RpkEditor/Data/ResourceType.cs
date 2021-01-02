using SLStudio.RpkEditor.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    public class ResourceType
    {
        private static readonly IReadOnlyCollection<ResourceType> types = new List<ResourceType>()
        {
            new ResourceType(CommonResources.Mesh, 5, "Cube"),
            new ResourceType("Sound", 6, ""),
            new ResourceType("Texture", 7, ""),
            new ResourceType("Game Object", 8, ""),
            new ResourceType("Click", 9, ""),
            new ResourceType("Light", 13, ""),
            new ResourceType("Render", 14, ""),
            new ResourceType("Animation", 19, "")
        };

        public ResourceType(string name, int id, string icon)
        {
            Name = name;
            Id = id;
            Icon = icon;
        }

        public string Name { get; }

        public int Id { get; }

        public string Icon { get; }

        public static IEnumerable<ResourceType> AvaliableTypes => types.OrderBy(t => t.Id);

        public static ResourceType MeshType => types.FirstOrDefault(t => t.Id == 5);

        public static ResourceType SoundType => types.FirstOrDefault(t => t.Id == 6);

        public static ResourceType TextureType => types.FirstOrDefault(t => t.Id == 7);

        public static ResourceType GameObjectType => types.FirstOrDefault(t => t.Id == 8);

        public static ResourceType ClickType => types.FirstOrDefault(t => t.Id == 9);

        public static ResourceType LightType => types.FirstOrDefault(t => t.Id == 13);

        public static ResourceType RenderType => types.FirstOrDefault(t => t.Id == 14);

        public static ResourceType AnimationType => types.FirstOrDefault(t => t.Id == 19);

        public static ResourceType FromId(int id)
        {
            return types.FirstOrDefault(r => r.Id == id);
        }

        public static ResourceType FromName(string name)
        {
            return types.FirstOrDefault(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}