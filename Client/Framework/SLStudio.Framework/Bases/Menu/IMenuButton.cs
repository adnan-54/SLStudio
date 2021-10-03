using System.Collections.Generic;
using System.Windows.Input;

namespace SLStudio
{
    public interface IMenuButton : IMenuItem
    {
        string Title { get; }

        string Tooltip { get; }

        object Shortcut { get; }

        object CommandParameter { get; }

        ICommand Command { get; }

        bool IsEnabled { get; }

        object Icon { get; }

        IEnumerable<IMenuItem> Children { get; }
    }
}