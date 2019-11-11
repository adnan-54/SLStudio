using SLStudio.Studio.Core.Framework.Commands;

namespace SLStudio.Studio.Core.Modules.Logging.Commands
{
    [CommandDefinition]
    public class OpenLogsCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.Logs";

        public override string Name => CommandName;

        public override string Text => "Logs";

        public override string ToolTip => "Logs";
    }
}
