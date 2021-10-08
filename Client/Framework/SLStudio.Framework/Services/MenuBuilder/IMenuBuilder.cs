using System.Collections.Generic;

namespace SLStudio
{
    public interface IMenuBuilder
    {
        IReadOnlyDictionary<string, IMenuItem> Menus { get; }

        IEnumerable<IMenuItem> BuildMenus();
    }
}