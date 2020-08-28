using System.Collections.Generic;

namespace SLStudio.RpkEditor.Data
{
    internal class ExternalReferenceMetadata
    {
        public ExternalReferenceMetadata(string path, string alias, string targetVersion, IReadOnlyCollection<ExternalResourceMetadata> metadatas)
        {
            Path = path;
            Alias = alias;
            TargetVersion = targetVersion;
            Metadatas = metadatas;
        }

        public string Path { get; }

        public string Alias { get; }

        public string TargetVersion { get; }

        public IReadOnlyCollection<ExternalResourceMetadata> Metadatas { get; }
    }
}