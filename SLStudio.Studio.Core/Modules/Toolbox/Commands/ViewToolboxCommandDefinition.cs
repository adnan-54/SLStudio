using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Properties;

namespace SLStudio.Studio.Core.Modules.Toolbox.Commands
{
    [CommandDefinition]
    public class ViewToolboxCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.Toolbox";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.ViewToolboxCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.ViewToolboxCommandToolTip; }
        }
    }
}