using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Shell.Commands
{
    [CommandHandler]
    public class ExitCommandHandler : CommandHandlerBase<ExitCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ExitCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.Close();
            return TaskUtility.Completed;
        }
    }
}