using System.Collections.Generic;

namespace SLStudio
{
    public interface IMenuBuilder : IService
    {
        IReadOnlyDictionary<string, IMenuItem> Items { get; }

        IEnumerable<IMenuItem> BuildMenus();
    }
}