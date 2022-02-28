using SLStudio.Studio.Commands;
using SLStudio.Studio.Menu;

namespace SLStudio.Studio;

internal class StudioModule : StudioModuleBase
{
    protected override void OnConfigure(IConfigurationContext context)
    {
        context.AddSubModule<CommandsModule>();
        context.AddSubModule<MenuModule>();
    }
}
