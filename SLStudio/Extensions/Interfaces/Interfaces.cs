using SLStudio.ViewsExtensions.Themes;

namespace SLStudio.Interfaces
{
    public interface ITheme
    {
        Theme Theme { get; set; }
    }

    public interface IThemedWindow: ITheme
    {
        void UpdateTheme();
    }

    public interface IStartScreen
    {
        void OpenFromRecentList(string path);
        void CloseFromChild();
    }

    public interface IMultiLanguageWindow
    {
        void UpdateLanguage();
    }
}
