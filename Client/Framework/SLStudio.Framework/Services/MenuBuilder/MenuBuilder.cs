using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class MenuBuilder : Service, IMenuBuilder
    {
        private readonly IMenuItemFactory menuItemFactory;
        private readonly Dictionary<string, IMenuItem> menus;

        public MenuBuilder(IMenuItemFactory menuItemFactory, IMessenger messenger)
        {
            this.menuItemFactory = menuItemFactory;
            menus = new Dictionary<string, IMenuItem>();

            messenger.Register<MenuConfigurationRegisteredMessage>(this, OnConfigurationRegistered);
        }

        public IReadOnlyDictionary<string, IMenuItem> Items => menus;

        public IEnumerable<IMenuItem> BuildMenus()
        {
            var orderedMenus = menus.Select(kvp => kvp.Value).OrderBy(m => m.Path?.Length);

            foreach (var menu in orderedMenus)
            {
                var parentPath = menu.GetParentPath();

                if (parentPath is null)
                    continue;

                if (!menus.TryGetValue(parentPath, out var parent))
                    throw new InvalidOperationException($"Parent for menu item '{menu.Path}' not found");

                parent.AddChild(menu);
            }

            return menus.Select(kvp => kvp.Value).Where(m => m.Parent is null).OrderBy(m => m.Index);
        }

        private void OnConfigurationRegistered(MenuConfigurationRegisteredMessage message)
        {
            var configuration = message.MenuConfiguration;
            foreach (var menu in configuration.BuildMenu(menuItemFactory))
                menus.Add(menu.Path, menu);
        }
    }
}