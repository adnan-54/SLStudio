using SLStudio.Core.Modules.LogsVisualizer.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ViewLogsHandler : MenuCommandHandler
    {
        private readonly IWindowManager windowManager;

        public ViewLogsHandler(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            windowManager.ShowDialog<LogVisualizerViewModel>();
            return Task.CompletedTask;
        }
    }
}