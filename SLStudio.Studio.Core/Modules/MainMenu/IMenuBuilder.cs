using SLStudio.Studio.Core.Framework.Menus;
using SLStudio.Studio.Core.Modules.MainMenu.Models;

namespace SLStudio.Studio.Core.Modules.MainMenu
{
    public interface IMenuBuilder
    {
        void BuildMenuBar(MenuBarDefinition menuBarDefinition, MenuModel result);
    }
}