namespace SLStudio.Data.Rdf
{
    public class RdfFileMetadata
    {
        internal RdfFileMetadata()
        {
        }

        public RdfFileMetadata(string logicalPath, string targetVersion, string author = null, string fileVersion = null, string studioVersion = null, string password = null)
        {
            Author = author;
            LogicalPath = logicalPath;
            TargetVersion = targetVersion;
            FileVersion = fileVersion;
            StudioVersion = studioVersion;
            Password = password;
        }

        public string Author { get; private set; }
        public string LogicalPath { get; private set; }
        public string TargetVersion { get; private set; }
        public string FileVersion { get; private set; }
        public string StudioVersion { get; private set; }
        public string Password { get; private set; }
    }
}