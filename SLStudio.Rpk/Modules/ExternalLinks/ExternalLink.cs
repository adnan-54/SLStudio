namespace SLStudio.Rpk.Modules.ExternalLinks
{
    public interface IExternalLink
    {
        string Path { get; set; }
    }

    class ExternalLink : IExternalLink
    {
        public ExternalLink(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}
