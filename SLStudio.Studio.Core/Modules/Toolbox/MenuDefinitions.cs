using SLStudio.Studio.Core.Framework.Menus;
using SLStudio.Studio.Core.Modules.Toolbox.Commands;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.Toolbox
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewToolboxMenuItem = new CommandMenuItemDefinition<ViewToolboxCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 4);
    }
}