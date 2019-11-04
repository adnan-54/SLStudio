using SLStudio.Studio.Core.Framework.Commands;

namespace SLStudio.Studio.Core.Modules.Shell.Commands
{
    [CommandDefinition]
    public class SwitchToDocumentCommandListDefinition : CommandListDefinition
    {
        public const string CommandName = "Window.SwitchToDocument";

        public override string Name
        {
            get { return CommandName; }
        }
    }
}