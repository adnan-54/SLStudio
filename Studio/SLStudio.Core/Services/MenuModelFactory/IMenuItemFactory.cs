using System;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    public interface IMenuItemFactory
    {
        IMenuItem MenuItem(string path, int index, string displayName, string toolTip, object iconSource, bool isVisible, bool isEnabled);

        IMenuItem MenuItem<THandler>(string path, int index, string displayName, string toolTip, object iconSource, KeyGesture shortcut, bool isVisible, bool isEnabled)
            where THandler : class, IMenuCommandHandler;

        IMenuToggleItem MenuToggle<THandler>(string path, int index, string displayName, string toolTip, object iconSource, KeyGesture shortcut, bool isVisible, bool isEnabled, bool isChecked)
            where THandler : class, IMenuCommandHandler;

        IMenuSeparatorItem MenuSeparator(string path, int index);
    }
}