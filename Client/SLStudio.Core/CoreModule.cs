using SLStudio.Modules.Shell.Views;

namespace SLStudio;

internal class CoreModule : StudioModule
{
    public override int Priority => int.MaxValue;

    protected override void OnConfigure(IConfigurationContext context)
    {
        context.AddViewModel<IShell, ShellViewModel>(Lifestyle.Singleton);
        context.AddView<ShellView, IShell>();
    }
}
