namespace SLStudio;

internal class SLStudioModule : StudioModule
{
    protected override void OnConfigure(IApplicationContext context)
    {
        context.AddTransient();
    }
}
