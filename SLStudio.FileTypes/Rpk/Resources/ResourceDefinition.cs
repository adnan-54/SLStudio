namespace SLStudio.FileTypes.RpkFile
{
    public abstract class ResourceDefinition
    {
        internal virtual int AdditionalType => 0;
        internal abstract ResourceType TypeOfEntry { get; }
    }
}