using Caliburn.Micro;
using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Framework.Threading;
using SLStudio.Studio.Core.Modules.Logging.ViewModels;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Logging.Commands
{
    [CommandHandler]
    public class OpenLogsCommandHandler : CommandHandlerBase<OpenLogsCommandDefinition>
    {
        private readonly IWindowManager windowManager;

        [ImportingConstructor]
        public OpenLogsCommandHandler(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public override Task Run(Command command)
        {
            windowManager.ShowDialog(IoC.Get<LoggerViewModel>());
            return TaskUtility.Completed;
        }
    }
}
