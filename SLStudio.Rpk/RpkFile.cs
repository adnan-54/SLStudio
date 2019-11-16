using SLStudio.Rpk.Modules.ExternalLinks;

namespace SLStudio.Rpk
{
    public interface IRpkFile
    {
        IExternalLinks ExternalLinks { get; }
    }

    class RpkFile : IRpkFile
    {
        public RpkFile()
        {
            ExternalLinks = new ExternalLinks();
        }

        public IExternalLinks ExternalLinks { get; }
    }
}
