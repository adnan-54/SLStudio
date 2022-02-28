namespace SLStudio.Studio.Menu;

internal class MenuModule : ISubModule
{
    public void OnConfigure(IConfigurationContext context)
    {
        context.AddMenuConfiguration<StudioMenuConfiguration>();
    }
}
