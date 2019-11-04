using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Properties;

namespace SLStudio.Studio.Core.Modules.Shell.Commands
{
    [CommandDefinition]
    public class SaveFileAsCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.SaveFileAs";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.FileSaveAsCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.FileSaveAsCommandToolTip; }
        }
    }
}