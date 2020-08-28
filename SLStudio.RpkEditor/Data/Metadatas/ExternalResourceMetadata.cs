namespace SLStudio.RpkEditor.Data
{
    internal class ExternalResourceMetadata
    {
        public int TypeId { get; }
        public int SuperId { get; }
        public int AdditionalType { get; }
        public string Alias { get; }
        public bool IsParentCompatible { get; }
        public ResourceType TypeOfEntry { get; }
    }
}