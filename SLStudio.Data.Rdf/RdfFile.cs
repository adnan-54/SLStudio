using System.IO;

namespace SLStudio.Data.Rdf
{
    public class RdfFile : IRdfFile
    {
        public string FilePath { get; init; }

        public string Name => string.IsNullOrEmpty(FilePath) ? string.Empty : Path.GetFileName(FilePath);

        public RdfFileMetadata Metadata { get; set; }

        public string Content { get; set; }

        internal static RdfFileBuilder Builder => new RdfFileBuilder();

        internal class RdfFileBuilder
        {
            private string filePath;
            private RdfFileMetadata metadata;
            private string content;

            public RdfFileBuilder With(string filePath, string content)
            {
                this.filePath = Path.GetFullPath(filePath);
                this.content = content;
                return this;
            }

            public RdfFileBuilder With(RdfFileMetadata metadata)
            {
                this.metadata = metadata;
                return this;
            }

            public IRdfFile Create()
            {
                if (metadata == null)
                    metadata = new RdfFileMetadata();

                return new RdfFile()
                {
                    FilePath = filePath,
                    Content = content,
                    Metadata = metadata
                };
            }
        }
    }
}