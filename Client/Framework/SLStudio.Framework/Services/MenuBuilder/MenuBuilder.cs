using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class MenuBuilder : Service, IMenuBuilder
    {
        private readonly IMenuItemFactory menuItemFactory;
        private readonly List<IMenuItem> menus;

        public MenuBuilder(IMenuItemFactory menuItemFactory, IMessenger messenger)
        {
            this.menuItemFactory = menuItemFactory;
            menus = new List<IMenuItem>();

            messenger.Register<MenuConfigurationRegisteredMessage>(this, OnConfigurationRegistered);
        }

        public IReadOnlyDictionary<string, IMenuItem> Menus { get; private set; }

        public IEnumerable<IMenuItem> BuildMenus()
        {
            foreach (var menu in menus.OrderBy(m => m.HierarchicalLevel))
            {
                if (menu.IsRootItem)
                    continue;

                if (!Menus.TryGetValue(menu.ParentPath, out var parent))
                    throw new InvalidOperationException($"Cannot find parent for menu item '{menu.Path}'");

                parent.AddChild(menu);
            }

            return menus.Where(m => m.IsRootItem).OrderBy(m => m.Index);
        }

        private void OnConfigurationRegistered(MenuConfigurationRegisteredMessage message)
        {
            var configuration = message.MenuConfiguration;
            menus.AddRange(configuration.BuildMenu(menuItemFactory));
            Menus = menus.ToDictionary(m => m.Path, m => m);
        }
    }
}