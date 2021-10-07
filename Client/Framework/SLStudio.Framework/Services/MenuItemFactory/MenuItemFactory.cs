using System.Windows.Input;

namespace SLStudio
{
    internal class MenuItemFactory : Service, IMenuItemFactory
    {
        private readonly IObjectFactory objectFactory;

        public MenuItemFactory(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        IMenuItem IMenuItemFactory.CreateItem(string path, int? index, string title, string toolTip, object icon, bool isVisible, bool isEnabled)
        {
            return new MenuItem()
            {
                Path = path,
                Index = index,
                Title = title,
                ToolTip = toolTip,
                Icon = icon,
                IsVisible = isVisible,
                IsEnabled = isEnabled,
            };
        }

        IMenuButton IMenuItemFactory.CreateButton<THandler>(string path, int? index, string title, string toolTip, object icon, bool isVisible, bool isEnabled, KeyGesture shortcut)
        {
            var handler = objectFactory.Create<THandler>();
            var button = new MenuButton(handler)
            {
                Path = path,
                Index = index,
                Title = title,
                ToolTip = toolTip,
                Icon = icon,
                IsVisible = isVisible,
                IsEnabled = isEnabled,
                Shortcut = shortcut
            };
            handler.SetMenu(button);

            return button;
        }

        IMenuToggle IMenuItemFactory.CreateToggle<THandler>(string path, int? index, string title, string toolTip, object icon, bool isVisible, bool isEnabled, bool isChecked, KeyGesture shortcut)
        {
            var handler = objectFactory.Create<THandler>();
            var toggle = new MenuToggle(handler)
            {
                Path = path,
                Index = index,
                Title = title,
                ToolTip = toolTip,
                Icon = icon,
                IsVisible = isVisible,
                IsEnabled = isEnabled,
                IsChecked = isChecked,
                Shortcut = shortcut,
            };
            handler.SetMenu(toggle);

            return toggle;
        }

        IMenuSeparator IMenuItemFactory.CreateSeparator(string path, int? index, bool isVisible)
        {
            return new MenuSeparator()
            {
                Path = path,
                Index = index,
                IsVisible = isVisible
            };
        }
    }
}
