using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Input;

namespace SLStudio
{
    public abstract class MenuConfiguration : IMenuConfiguration
    {
        private readonly List<IMenuItem> items;
        private IMenuItemFactory menuItemFactory;
        private ResourceManager resourceManager;

        protected MenuConfiguration()
        {
            items = new List<IMenuItem>();
        }

        public IEnumerable<IMenuItem> BuildMenu(IMenuItemFactory menuItemFactory)
        {
            if (!items.Any())
            {
                this.menuItemFactory = menuItemFactory;
                Build();
            }

            return items;
        }

        protected abstract void Build();

        protected void SetResourceContext(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        protected IMenuItem Item(string path, int? index = null, string title = null, string toolTip = null, object icon = null, bool isVisible = true, bool isEnabled = true)
        {
            CheckMenuItemFactory();

            var item = menuItemFactory.CreateItem(path, index, title ?? GetTitle(path), toolTip ?? GetToolTip(path), icon, isVisible, isEnabled);
            items.Add(item);

            return item;
        }

        protected IMenuButton Button<THandler>(string path, int? index = null, string title = null, string toolTip = null, object icon = null, bool isVisible = true, bool isEnabled = true, KeyGesture shortcut = null)
            where THandler : class, IMenuButtonHandler
        {
            CheckMenuItemFactory();

            var button = menuItemFactory.CreateButton<THandler>(path, index, title ?? GetTitle(path), toolTip ?? GetToolTip(path), icon, isVisible, isEnabled, shortcut);
            items.Add(button);

            return button;
        }

        protected IMenuToggle Toggle<THandler>(string path, int? index = null, string title = null, string toolTip = null, object icon = null, bool isVisible = true, bool isEnabled = true, bool isChecked = false, KeyGesture shortcut = null)
            where THandler : class, IMenuToggleHandler
        {
            CheckMenuItemFactory();

            var toggle = menuItemFactory.CreateToggle<THandler>(path, index, title ?? GetTitle(path), toolTip ?? GetToolTip(path), icon, isVisible, isEnabled, isChecked, shortcut);
            items.Add(toggle);

            return toggle;
        }

        protected IMenuSeparator Separator(string path, int? index = null, bool isVisible = true)
        {
            CheckMenuItemFactory();

            var separator = menuItemFactory.CreateSeparator(path, index, isVisible);
            items.Add(separator);

            return separator;
        }

        private void CheckMenuItemFactory()
        {
            if (menuItemFactory is null)
                throw new InvalidOperationException($"{nameof(IMenuItemFactory)} is null");
        }

        private string GetTitle(string path)
        {
            return resourceManager?.GetString($"{path}_title") ?? path.Split('|', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        }

        private string GetToolTip(string path)
        {
            return resourceManager?.GetString($"{path}_tooltip");
        }
    }
}