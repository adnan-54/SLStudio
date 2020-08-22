namespace SLStudio.RpkEditor.Data
{
    internal class ReferenceDefinitionsMetadata : ResourceMetadata
    {
        public override string IconSource => "Link";

        public override string DisplayName => "Reference";

        public override string Category => "References";

        public override int AdditionalType { get; }

        public override ResourceType TypeOfEntry { get; }
    }
}