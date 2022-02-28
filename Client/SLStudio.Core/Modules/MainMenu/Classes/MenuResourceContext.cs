using System.Resources;
using Humanizer;

namespace SLStudio;

internal class MenuResourceContext : IMenuResourceContext
{
    private readonly List<ResourceManager> resources;

    public MenuResourceContext()
    {
        resources = new();
    }

    public void AddResourceManager(ResourceManager resourceManager)
    {
        if (resources.Contains(resourceManager))
            return;
        resources.Add(resourceManager);
    }

    public string GetTitle(string path)
    {
        var title = GetFromResource($"{path}_title");
        return title ?? GetFromPath(path);
    }

    public string? GetToolTip(string path)
    {
        return GetFromResource($"{path}_tooltip");
    }

    private string? GetFromResource(string key)
    {
        foreach (var resourceManager in resources)
        {
            var title = resourceManager.GetString(key);
            if (!string.IsNullOrEmpty(title))
                return title;
        }
        return null;
    }

    private string GetFromPath(string path)
    {
        var lastSeparatorIndex = path.LastIndexOf('|');
        if (lastSeparatorIndex >= 0)
            path = path[lastSeparatorIndex..];
        return path.Titleize();
    }
}
