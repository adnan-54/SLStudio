using SLStudio.Studio.Core.Framework.ToolBars;
using SLStudio.Studio.Core.Properties;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Modules.ToolBars
{
    internal static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarDefinition StandardToolBar = new ToolBarDefinition(0, Resources.ToolBarStandard);
    }
}