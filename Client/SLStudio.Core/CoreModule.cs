namespace SLStudio;

internal class CoreModule : StudioModule
{
    public override int Priority => int.MaxValue;

    protected override void OnConfigure(IConfigurationContext context)
    {
        context.AddResource(new Uri("pack://application:,,,/SLStudio.Core;component/Modules/Resources/Xaml/Styles.xaml"));

        context.AddViewModel<IMainMenu, MainMenuViewModel>(Lifestyle.Singleton);
        context.AddView<MainMenuView, IMainMenu>();

        context.AddViewModel<IShell, ShellViewModel>(Lifestyle.Singleton);
        context.AddView<ShellView, IShell>();
    }
}
