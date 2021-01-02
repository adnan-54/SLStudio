namespace SLStudio.Data.Rdf
{
    public interface IRdfFile
    {
        string FilePath { get; }

        string Name { get; }

        RdfFileMetadata Metadata { get; set; }

        string Content { get; set; }
    }
}