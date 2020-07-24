using System.Collections.Generic;

namespace SLStudio.FileTypes.RpkFile
{
    public class Rpk : GameFile
    {
        public Rpk()
        {
            ExternalRefs = new List<Rpk>();
            Meshes = new List<Resource<MeshDefinition>>();
            Sounds = new List<Resource<SoundDefinition>>();
        }

        public List<Rpk> ExternalRefs { get; }

        public List<Resource<MeshDefinition>> Meshes { get; }

        public List<Resource<SoundDefinition>> Sounds { get; }
    }
}