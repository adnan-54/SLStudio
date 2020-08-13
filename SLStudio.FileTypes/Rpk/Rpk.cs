using System.Collections.Generic;

namespace SLStudio.FileTypes.RpkFile
{
    public class Rpk : GameFile
    {
        public Rpk()
        {
            ExternalRefs = new List<Rpk>();
            Resources = new List<ResourceBase>();
        }

        public List<Rpk> ExternalRefs { get; }

        public List<ResourceBase> Resources { get; }
    }
}