using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Properties;

namespace SLStudio.Studio.Core.Modules.Shell.Commands
{
    [CommandDefinition]
    public class CloseFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.CloseFile";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.FileCloseCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.FileCloseCommandToolTip; }
        }
    }
}