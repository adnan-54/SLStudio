using System.Collections.Generic;

namespace SLStudio.RpkEditor.Data
{
    internal class ExternalReferenceMetadata
    {
        private readonly RpkMetadata parent;

        public ExternalReferenceMetadata(RpkMetadata parent)
        {
            this.parent = parent;
        }

        public string Path { get; }

        public string TargetVersion { get; }

        public int Id => parent.ExternalReferences.IndexOf(this);

        public IReadOnlyCollection<ReferenceDefinitionsMetadata> Metadatas { get; }
    }
}