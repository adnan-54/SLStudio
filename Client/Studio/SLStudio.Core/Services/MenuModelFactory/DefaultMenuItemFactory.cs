using SLStudio.Core.Library.Menus;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class DefaultMenuItemFactory : IMenuItemFactory
    {
        private readonly IObjectFactory objectFactory;
        private int lastIndex;

        public DefaultMenuItemFactory(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public IMenuItem MenuItem(string path, int index, string displayName, string toolTip, object iconSource, bool isVisible, bool isEnabled)
        {
            if (index < 0)
                index = lastIndex++;

            return new MenuItem(path, index, displayName, toolTip, iconSource, null, isVisible, isEnabled);
        }

        IMenuItem IMenuItemFactory.MenuItem<THandler>(string path, int index, string displayName, string toolTip, object iconSource, KeyGesture shortcut, bool isVisible, bool isEnabled)
        {
            if (index < 0)
                index = lastIndex++;

            var item = new MenuItem(path, index, displayName, toolTip, iconSource, shortcut, isVisible, isEnabled);
            item.Command = new MenuCommandWrapper(item, objectFactory.Create<THandler>());

            return item;
        }

        IMenuToggleItem IMenuItemFactory.MenuToggle<THandler>(string path, int index, string displayName, string toolTip, object iconSource, KeyGesture shortcut, bool isVisible, bool isEnabled, bool isChecked)
        {
            if (index < 0)
                index = lastIndex++;

            var toggle = new MenuToggleItem(path, index, displayName, toolTip, iconSource, shortcut, isVisible, isEnabled, isChecked);
            var handler = objectFactory.Create<THandler>();
            toggle.Command = new MenuCommandWrapper(toggle, handler);
            toggle.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(toggle.IsChecked))
                    handler.OnToggle(toggle, handler);
            };
            return toggle;
        }

        public IMenuSeparatorItem MenuSeparator(string path, int index)
        {
            if (index < 0)
                index = lastIndex++;

            return new MenuSeparatorItem(path, index);
        }
    }
}