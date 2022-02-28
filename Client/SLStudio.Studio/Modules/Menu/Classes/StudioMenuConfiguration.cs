using SLStudio.Studio.Commands;

namespace SLStudio.Studio.Menu;

internal class StudioMenuConfiguration : MenuConfiguration
{
    public override void BuildMenu(IMenuBuilder builder)
    {
        builder.UseResourceContext(MenuResources.ResourceManager);
        builder.Item("file");
        builder.Item("file|new");
        builder.Button<CreateNewFileCommand>("file|new|file");
    }
}
