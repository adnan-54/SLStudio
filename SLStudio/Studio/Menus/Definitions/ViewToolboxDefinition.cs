using Gemini.Framework.Commands;

namespace SLStudio.Studio.Menus.Definition
{
    [CommandDefinition]
    public class ViewToolboxDefinition : CommandDefinition
    {
        public const string CommandName = "View.Toolbox";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "test"; }
        }

        public override string ToolTip
        {
            get { return "dale"; }
        }
    }
}
