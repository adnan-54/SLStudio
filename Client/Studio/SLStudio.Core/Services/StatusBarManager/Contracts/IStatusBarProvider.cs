using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface IStatusBarProvider
    {
        IEnumerable<IStatusBarContent> LeftContent { get; }
        IEnumerable<IStatusBarContent> CenterContent { get; }
        IEnumerable<IStatusBarContent> RightContent { get; }

        void Refresh();
    }
}