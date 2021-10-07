using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class MenuBuilder : Service, IMenuBuilder
    {
        private readonly IObjectFactory objectFactory;
        private readonly IMenuItemFactory menuItemFactory;
        private readonly List<Type> configurationsTypes;

        public MenuBuilder(IObjectFactory objectFactory, IMenuItemFactory menuItemFactory, IMessenger messenger)
        {
            this.objectFactory = objectFactory;
            this.menuItemFactory = menuItemFactory;
            configurationsTypes = new List<Type>();

            messenger.Register<MenuConfigurationRegisteredMessage>(this, OnConfigurationRegistered);
        }

        public IReadOnlyDictionary<string, IMenuItem> Menus { get; private set; }

        public IEnumerable<IMenuItem> BuildMenus()
        {
            var configurations = configurationsTypes.Select(t => objectFactory.Create<IMenuConfiguration>(t));
            var menus = configurations.SelectMany(c => c.BuildMenu(menuItemFactory)).OrderBy(m => m.HierarchicalLevel).ToList();
            Menus = menus.ToDictionary(m => m.Path, m => m);

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
            configurationsTypes.Add(message.MenuConfigurationType);
        }
    }
}