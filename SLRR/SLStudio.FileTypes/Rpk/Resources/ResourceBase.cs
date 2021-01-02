using System.Threading;

namespace SLStudio.FileTypes.RpkFile
{
    public abstract class ResourceBase
    {
        public Rpk Parent { get; set; }
        public int TypeId { get; set; }
        public int SuperId { get; set; }
        public virtual int AdditionalType { get; set; }
        public string Alias { get; set; }
        public bool IsParentCompatible { get; set; }
        public abstract ResourceType TypeOfEntry { get; }
    }
}