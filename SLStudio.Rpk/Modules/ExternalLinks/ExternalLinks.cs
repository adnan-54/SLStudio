using System.Collections.Generic;

namespace SLStudio.Rpk.Modules.ExternalLinks
{
    public interface IExternalLinks
    {
        IList<IExternalLink> Links { get; }
    }

    class ExternalLinks : IExternalLinks
    {
        private readonly List<IExternalLink> links;

        public ExternalLinks()
        {
            links = new List<IExternalLink>();
        }

        public IList<IExternalLink> Links => links;
    }
}
