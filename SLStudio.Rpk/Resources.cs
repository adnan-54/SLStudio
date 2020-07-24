namespace SLStudio.Rpk
{
    public abstract class ResourceBase
    {
        public int TypeId { get; set; }
        public abstract int SuperId { get; }
        public int AdditionalType { get; set; }
        public string Alias { get; set; }
        public bool IsParentCompatible { get; set; }
        public abstract ResourceType TypeOfEntry { get; }
    }

    public class Resource<T> : ResourceBase where T : ResourceDefinition
    {
        public ResourceBase Parent { get; set; }

        public override int SuperId => Parent.TypeId;

        public override ResourceType TypeOfEntry => Definition.TypeOfEntry;

        public T Definition { get; set; }
    }
}