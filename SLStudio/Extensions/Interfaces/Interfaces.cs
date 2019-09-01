using SLStudio.ViewsExtensions.Themes;

namespace SLStudio.Extensions.Interfaces
{
    public interface IThemedControl
    {
        Theme Theme { get; set; }
        void UpdateTheme();
    }

    public interface IMultiLanguageControl
    {
        void UpdateLanguage();
    }

    public interface IStartScreen
    {
        void OpenFromRecentList(string path);
        void CloseFromChild();
    }
}
