using SLStudio.RpkEditor.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    public class ResourceType
    {
        private static readonly ResourceType instance = new ResourceType();

        private readonly IReadOnlyCollection<ResourceType> types;

        private ResourceType()
        {
            types = new List<ResourceType>()
            {
                new ResourceType(){ Name = CommonResources.Mesh, Id = 5 }
            };
        }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public static IEnumerable<ResourceType> AvaliableTypes => instance.types.OrderBy(t => t.Id);

        public static ResourceType MeshType => instance.types.FirstOrDefault(t => t.Id == 5);

        public static ResourceType FromId(int id)
        {
            return instance.types.FirstOrDefault(r => r.Id == id);
        }

        public static ResourceType FromName(string name)
        {
            return instance.types.FirstOrDefault(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}