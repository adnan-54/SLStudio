using SLStudio.Studio.Core.Framework.Menus;
using SLStudio.Studio.Core.Modules.Output.Commands;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.Output
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewOutputMenuItem = new CommandMenuItemDefinition<ViewOutputCommandDefinition>(MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);
    }
}
