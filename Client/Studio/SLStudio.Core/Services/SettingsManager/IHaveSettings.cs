using System.Configuration;

namespace SLStudio.Core
{
    public interface IHaveSettings
    {
        ApplicationSettingsBase Settings { get; }
    }
}