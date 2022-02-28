using System.Resources;
using System.Text.RegularExpressions;

namespace SLStudio;

internal class MenuBuilder : IMenuBuilder
{
    private readonly IMenuItemFactory menuItemFactory;
    private readonly Dictionary<string, IMenuItem> itemsCache;
    private readonly IMenuResourceContext resourceContext;

    public MenuBuilder(IMenuItemFactory menuItemFactory)
    {
        this.menuItemFactory = menuItemFactory;
        itemsCache = new(StringComparer.OrdinalIgnoreCase);
        resourceContext = new MenuResourceContext();
    }

    void IMenuBuilder.UseResourceContext(ResourceManager resourceManager)
    {
        resourceContext.AddResourceManager(resourceManager);
    }

    void IMenuBuilder.Item(string path, int? index, string? title, string? toolTip, object? icon)
    {
        SetCommomProperties(path, ref index, ref title, ref toolTip, ref icon);
        var item = menuItemFactory.Item(path, index!.Value, title!, toolTip, icon!);
        itemsCache.Add(path, item);
        ResolveNesting(item);
    }

    void IMenuBuilder.Button<THandler>(string path, string commandKey, int? index, string? title, string? toolTip, object? icon)
    {
        SetCommomProperties(path, ref index, ref title, ref toolTip, ref icon);
        var button = menuItemFactory.Button<THandler>(path, index!.Value, title!, toolTip, icon!, commandKey);
        itemsCache.Add(path, button);
        ResolveNesting(button);
    }

    void IMenuBuilder.Toggle<THandler>(string path, string commandKey, int? index, string? title, string? toolTip, object? icon)
    {
        SetCommomProperties(path, ref index, ref title, ref toolTip, ref icon);
        var toggle = menuItemFactory.Toggle<THandler>(path, index!.Value, title!, toolTip, icon!, commandKey);
        itemsCache.Add(path, toggle);
        ResolveNesting(toggle);
    }

    void IMenuBuilder.Separator(string path, int? index)
    {
        ValidatePath(path);
        if (!index.HasValue)
            index = GetIndexFromPath(path);
        var separator = menuItemFactory.Separator(path, index.Value);
        ResolveNesting(separator);
    }

    public IEnumerable<IMenuItem> Build()
    {
        return itemsCache.Values.Where(m => m.IsRootItem).OrderBy(m => m.Index);
    }

    private void SetCommomProperties(string path, ref int? index, ref string? title, ref string? toolTip, ref object? icon)
    {
        ValidatePath(path);
        if (!index.HasValue)
            index = GetIndexFromPath(path);
        if (string.IsNullOrEmpty(title))
            title = GetTitleFromPath(path);
        if (string.IsNullOrEmpty(toolTip))
            toolTip = GetToolTipFromPath(path);
        if (icon is null)
            icon = new object();
    }

    private void ValidatePath(string path)
    {
        if (itemsCache.ContainsKey(path))
            throw new Exception($"Path '{path}' has been already used");
        if (string.IsNullOrWhiteSpace(path))
            throw new Exception("Path cannot be empty");
        if (path.StartsWith('|'))
            throw new Exception("Path cannot start with a separator");
        if (path.EndsWith('|'))
            throw new Exception("Path cannot end with a separator");
        if (!Regex.IsMatch(path, @"^[a-zA-Z0-9_]+$"))
        {
            var invalidChars = Regex.Replace(path, "[a-zA-Z0-9_]", "").ToCharArray().Select(c => $"'{c}'");
            var invalidString = string.Join(", ", invalidChars);
            throw new Exception($"Path contains invalid character(s): {invalidString}");
        }
        if (path.Contains('|') && !itemsCache.ContainsKey(GetParentPath(path)))
            throw new Exception($"Could not find a parent for path '{path}'");
    }

    private int GetIndexFromPath(string path)
    {
        if (!path.Contains('|'))
            return itemsCache.Values.Where(m => m.IsRootItem).Count();

        var parent = itemsCache[GetParentPath(path)];
        return parent.Children.Count();
    }

    private string GetTitleFromPath(string path)
    {
        return resourceContext.GetTitle(path);
    }

    private string? GetToolTipFromPath(string path)
    {
        return resourceContext.GetToolTip(path);
    }

    private void ResolveNesting(IMenuItem item)
    {
        if (item.IsRootItem)
            return;

        var parent = itemsCache[GetParentPath(item.Path)];
        parent.AddChild(item);
        item.SetParent(parent);
    }

    private static string GetParentPath(string path)
    {
        return path[..path.LastIndexOf('|')];
    }
}
