using System.Collections.Generic;

namespace SLStudio
{
    public interface IMenuConfiguration
    {
        IEnumerable<IMenuItem> BuildMenu(IMenuItemFactory menuItemFactory);
    }
}