using System.Configuration;

namespace SLStudio.Core
{
    public interface ISettingsService
    {
        void RegisterSettingFor(ViewModel viewModel, ApplicationSettingsBase settings);
    }
}
