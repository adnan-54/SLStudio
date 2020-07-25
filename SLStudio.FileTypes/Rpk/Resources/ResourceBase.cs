namespace SLStudio.FileTypes.RpkFile
{
    public abstract class ResourceBase
    {
        public Rpk ResourcePackage { get; set; }
        public int TypeId { get; set; }
        public abstract int SuperId { get; }
        public virtual int AdditionalType { get; set; }
        public string Alias { get; set; }
        public bool IsParentCompatible { get; set; }
        public abstract ResourceType TypeOfEntry { get; }
    }
}