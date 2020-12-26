using MvvmDialogs;
using SLStudio.Core.Modules.LogsVisualizer.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ViewLogsHandler : MenuCommandHandler
    {
        private readonly IDialogService dialogService;
        private readonly IWindowManager windowManager;

        public ViewLogsHandler(IDialogService dialogService, IWindowManager windowManager)
        {
            this.dialogService = dialogService;
            this.windowManager = windowManager;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            var vm = new LogVisualizerViewModel(dialogService, windowManager);
            windowManager.ShowDialog(vm);
            return Task.CompletedTask;
        }
    }
}