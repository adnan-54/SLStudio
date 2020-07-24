using System.Collections.Generic;

namespace SLStudio.Rpk
{
    public class Rpk
    {
        public string Path { get; set; }

        public List<Rpk> ExternalRefs;

        public List<Resource<MeshDefinition>> Meshes;

        public List<Resource<SoundDefinition>> Sounds;
    }
}