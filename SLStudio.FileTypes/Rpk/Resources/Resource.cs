namespace SLStudio.FileTypes.RpkFile
{
    public class Resource<T> : ResourceBase where T : ResourceDefinition
    {
        public ResourceBase Parent { get; set; }

        public override int SuperId => Parent.TypeId;

        public override int AdditionalType => Definition.AdditionalType;

        public override ResourceType TypeOfEntry => Definition.TypeOfEntry;

        public T Definition { get; set; }
    }
}