namespace SLStudio;

internal class CoreModule : StudioModule
{
    public override int Priority => int.MaxValue;

    protected override void OnConfigure(IConfigurationContext context)
    {
        context.AddViewModel<IShell, ShellViewModel>(Lifestyle.Singleton);
        context.AddView<ShellView, IShell>();

        context.AddResource(new Uri("pack://application:,,,/SLStudio.Core;component/Modules/Resources/Xaml/Styles.xaml"));
    }
}
