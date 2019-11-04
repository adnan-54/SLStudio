using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Toolbox.Commands
{
    [CommandHandler]
    public class ViewToolboxCommandHandler : CommandHandlerBase<ViewToolboxCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ViewToolboxCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool<IToolbox>();
            return TaskUtility.Completed;
        }
    }
}