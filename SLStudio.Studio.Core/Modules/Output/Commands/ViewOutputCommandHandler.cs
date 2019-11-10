using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Output.Commands
{
    [CommandHandler]
    public class ViewOutputCommandHandler : CommandHandlerBase<ViewOutputCommandDefinition>
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ViewOutputCommandHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override Task Run(Command command)
        {
            shell.ShowTool<IOutput>();
            return TaskUtility.Completed;
        }
    }
}
