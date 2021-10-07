using System.Windows.Input;

namespace SLStudio
{
    public interface IMenuItemFactory : IService
    {
        IMenuItem CreateItem(string path, int? index, string title, string toolTip, object icon, bool isVisible, bool isEnabled);

        IMenuButton CreateButton<THandler>(string path, int? index, string title, string toolTip, object icon, bool isVisible, bool isEnabled, KeyGesture shortcut)
            where THandler : class, IMenuButtonHandler;

        IMenuToggle CreateToggle<THandler>(string path, int? index, string title, string toolTip, object icon, bool isVisible, bool isEnabled, bool isChecked, KeyGesture shortcut)
            where THandler : class, IMenuToggleHandler;

        IMenuSeparator CreateSeparator(string path, int? index, bool isVisible);
    }
}