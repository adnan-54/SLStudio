using SLStudio.Studio.Core.Framework.Menus;
using SLStudio.Studio.Core.Modules.Logging.Commands;
using SLStudio.Studio.Core.Modules.Settings.Commands;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.Logging
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition OpenSettingsMenuItem = new CommandMenuItemDefinition<OpenLogsCommandDefinition>(
            MainMenu.MenuDefinitions.ToolsOptionsMenuGroup, 0);
    }
}
