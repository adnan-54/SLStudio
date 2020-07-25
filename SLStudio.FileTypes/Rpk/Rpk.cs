using System.Collections.Generic;

namespace SLStudio.FileTypes.RpkFile
{
    public class Rpk : GameFile
    {
        public Rpk()
        {
            ExternalRefs = new List<Rpk>();
            Resources = new List<ResourceBase>();
            Meshes = new List<Resource<MeshDefinition>>();
            Sounds = new List<Resource<SoundDefinition>>();
            Textures = new List<Resource<TextureDefinition>>();
            GameObjects = new List<Resource<GameObjectDefinition>>();
            Clicks = new List<Resource<ClickDefinition>>();
        }

        public List<Rpk> ExternalRefs { get; }

        public List<ResourceBase> Resources { get; }

        public List<Resource<MeshDefinition>> Meshes { get; }

        public List<Resource<SoundDefinition>> Sounds { get; }

        public List<Resource<TextureDefinition>> Textures { get; }

        public List<Resource<GameObjectDefinition>> GameObjects { get; }

        public List<Resource<ClickDefinition>> Clicks { get; }
    }
}