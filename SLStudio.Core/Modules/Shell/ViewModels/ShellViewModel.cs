namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : ViewModel, IShell
    {
        public ShellViewModel(ISettingsService settingsService)
        {
            settingsService.RegisterSettingFor(this, Resources.ShellSettings.Default);
            DisplayName = "SLStudio";
        }

        public string Test
        {
            get => GetProperty(() => Test);
            set => SetProperty(() => Test, value);
        }

        public void Test2()
        {
            Resources.ShellSettings.Default.Test = "Xesq do bresq";
        }
    }
}