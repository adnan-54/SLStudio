namespace SLStudio;

internal class CoreModule : StudioModule
{
    public override int Priority => int.MaxValue;

    protected override void OnConfigure(IConfigurationContext context)
    {
    }
}
