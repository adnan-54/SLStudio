using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    public abstract class MenuConfiguration : IMenuConfiguration
    {
        private readonly IMenuItemFactory menuItemFactory;
        private readonly Dictionary<string, IMenuItem> items;
        private ResourceManager resourceManager;

        public MenuConfiguration(IMenuItemFactory menuItemFactory)
        {
            items = new Dictionary<string, IMenuItem>();
            this.menuItemFactory = menuItemFactory;
        }

        public IEnumerable<IMenuItem> Items => GetItems();

        public abstract void Create();

        public IMenuItem GetItem(string path)
        {
            if (items.ContainsKey(path))
                return items[path];

            return null;
        }

        protected IMenuItem Item(string path, int index = -1, string displayName = null, string toolTip = null, object iconSource = null, bool isVisible = true, bool isEnabled = true)
        {
            ValidateItemPath(path);

            var item = menuItemFactory.MenuItem(path, index, displayName, toolTip, iconSource, isVisible, isEnabled);
            InsertItem(item);

            return item;
        }

        protected IMenuItem Item<THandler>(string path, int index = -1, string displayName = null, string toolTip = null, object iconSource = null, KeyGesture shortcut = null, bool isVisible = true, bool isEnabled = true) where THandler : class, IMenuCommandHandler
        {
            ValidateItemPath(path);

            var item = menuItemFactory.MenuItem<THandler>(path, index, displayName, toolTip, iconSource, shortcut, isVisible, isEnabled);
            InsertItem(item);

            return item;
        }

        protected IMenuItem Toggle<THandler>(string path, int index = -1, string displayName = null, string toolTip = null, object iconSource = null, KeyGesture shortcut = null, bool isVisible = true, bool isEnabled = true, bool isChecked = false) where THandler : class, IMenuCommandHandler
        {
            ValidateItemPath(path);

            var toggle = menuItemFactory.MenuToggle<THandler>(path, index, displayName, toolTip, iconSource, shortcut, isVisible, isEnabled, isChecked);
            InsertItem(toggle);

            return toggle;
        }

        protected IMenuItem Separator(string path, int index = -1)
        {
            ValidateItemPath(path);

            var separator = menuItemFactory.MenuSeparator(path, index);
            InsertItem(separator);

            return separator;
        }

        protected void SetResourceContext(Type resourceType)
        {
            resourceManager = new ResourceManager(resourceType);
        }

        private string DisplayNameFromPath(string path)
        {
            if (resourceManager == null)
                return path.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

            return ResourceHelpers.ResolveResouce(path, null, resourceManager);
        }

        private IEnumerable<IMenuItem> GetItems()
        {
            items.Clear();
            Create();
            SortMenus();
            return items.Values.Where(i => i.Parent == null).OrderBy(i => i.Index);
        }

        private void ValidateItemPath(string path)
        {
            if (items.ContainsKey(path))
                throw new InvalidOperationException($"The item '{path}' already exists");

            var parentPath = GetParentPath(path);
            if (!items.ContainsKey(parentPath) && !string.IsNullOrEmpty(parentPath))
                throw new InvalidOperationException($"Cannot create a sub-item of an item that doesn't exists. The item '{parentPath}' should be defined before it's child '{path}'");
        }

        private void InsertItem(IMenuItem item)
        {
            if (item.DisplayName == null)
                item.DisplayName = DisplayNameFromPath(item.Path);

            var parentPath = GetParentPath(item.Path);
            if (!string.IsNullOrEmpty(parentPath))
            {
                if (items.TryGetValue(parentPath, out IMenuItem parent))
                {
                    item.Parent = parent;
                    parent.Children.Add(item);
                }
            }
            items.Add(item.Path, item);
        }

        private static string GetParentPath(string path)
        {
            if (path.Contains('/'))
            {
                path = path.TrimEnd('/');
                return path.Remove(path.LastIndexOf('/'));
            }
            return string.Empty;
        }

        private void SortMenus()
        {
            foreach (var item in items.Values.Where(i => i.Children.Count > 0))
            {
                var children = item.Children.OrderBy(i => i.Index).ToList();
                item.Children.Clear();
                item.Children.AddRange(children);
            }
        }
    }

    public interface IMenuConfiguration
    {
        IEnumerable<IMenuItem> Items { get; }

        void Create();

        IMenuItem GetItem(string path);
    }
}