using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface IStatusBarManager
    {
        IEnumerable<IStatusBarHost> Hosts { get; }
        IEnumerable<IStatusBarContent> LeftContents { get; }
        IEnumerable<IStatusBarContent> CenterContents { get; }
        IEnumerable<IStatusBarContent> RightContents { get; }

        void Refresh();

        void AddHost(IStatusBarHost host);

        void RemoveHost(IStatusBarHost host);
    }
}