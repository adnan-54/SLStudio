using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using SLStudio.Studio.Menus.Definition;
using SLStudio.Studio.Modules.Tools.Toolbox;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SLStudio.Studio.Menus.Handlers
{
    [CommandHandler]
    public class ViewToolboxHandler : CommandHandlerBase<ViewToolboxDefinition>
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ViewToolboxHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override Task Run(Command command)
        {
            shell.ShowTool<IToolbox>();
            return TaskUtility.Completed;
        }
    }
}
