using Caliburn.Micro;
using SLStudio.Studio.Core.Framework.Commands;
using SLStudio.Studio.Core.Framework.Threading;
using SLStudio.Studio.Core.Modules.Settings.ViewModels;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Settings.Commands
{
    [CommandHandler]
    public class OpenSettingsCommandHandler : CommandHandlerBase<OpenSettingsCommandDefinition>
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public OpenSettingsCommandHandler(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public override Task Run(Command command)
        {
            _windowManager.ShowDialog(IoC.Get<SettingsViewModel>());
            return TaskUtility.Completed;
        }
    }
}