using System.Collections.Generic;

namespace SLStudio
{
    public interface IMenuBuilder : IService
    {
        IReadOnlyDictionary<string, IMenuItem> Menus { get; }

        IEnumerable<IMenuItem> BuildMenus();
    }
}