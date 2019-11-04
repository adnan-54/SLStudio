using SLStudio.Studio.Core.Framework.ToolBars;
using SLStudio.Studio.Core.Modules.Shell.Commands;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.Shell
{
    public static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarItemGroupDefinition StandardOpenSaveToolBarGroup = new ToolBarItemGroupDefinition(
            ToolBars.ToolBarDefinitions.StandardToolBar, 8);

        [Export]
        public static ToolBarItemDefinition OpenFileToolBarItem = new CommandToolBarItemDefinition<OpenFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 0);

        [Export]
        public static ToolBarItemDefinition SaveFileToolBarItem = new CommandToolBarItemDefinition<SaveFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 2);

        [Export]
        public static ToolBarItemDefinition SaveAllFilesToolBarItem = new CommandToolBarItemDefinition<SaveAllFilesCommandDefinition>(
            StandardOpenSaveToolBarGroup, 4);
    }
}