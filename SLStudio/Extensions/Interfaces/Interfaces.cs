using SLStudio.ViewsExtensions.Themes;

namespace SLStudio.Interfaces
{
    public interface ITheme
    {
        Theme Theme { get; set; }
    }

    public interface IStartScreen
    {
        void OpenFromRecentList(string path);
        void CloseFromChild();
    }
}
