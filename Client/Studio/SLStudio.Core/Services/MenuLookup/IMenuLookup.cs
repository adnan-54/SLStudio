using System.Collections.Generic;

namespace SLStudio.Core.Menus
{
    internal interface IMenuLookup
    {
        IEnumerable<IMenuConfiguration> Lookup();
    }
}