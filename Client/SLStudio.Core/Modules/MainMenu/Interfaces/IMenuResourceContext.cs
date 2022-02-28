using System.Resources;

namespace SLStudio;

public interface IMenuResourceContext
{
    void AddResourceManager(ResourceManager resourceManager);

    string GetTitle(string path);

    string? GetToolTip(string path);
}