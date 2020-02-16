using Caliburn.Micro;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core.Modules.Logs.ViewModels
{
    internal class LogsViewModel : Screen
    {
        private readonly ILoggingService loggingService;
        private readonly IWindowManager windowManager;

        private bool isBusy;
        private DataTable logs;

        public LogsViewModel(ILoggingService loggingService, IWindowManager windowManager)
        {
            this.loggingService = loggingService;
            this.windowManager = windowManager;
            FetchLogsAsync().FireAndForget();

            DisplayName = "Logs";
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public DataTable Logs
        {
            get => logs;
            set
            {
                logs = value;
                NotifyOfPropertyChange(() => Logs);
            }
        }

        public async void ClearLogs()
        {
            var result = MessageBox.Show(Resources.Logs.ClearConfirmationMessage, Resources.Logs.ClearAllLogs, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                IsBusy = true;
                await Task.Run(() =>
                {
                    loggingService.ClearAllLogs();
                });

                FetchLogsAsync().FireAndForget();
                IsBusy = false;
            }
        }

        public bool CanViewSimpleLog => loggingService.SimpleLogFileExists;

        public void ViewSimpleLog()
        {
            var model = new SimpleLogViewModel(loggingService);
            windowManager.ShowDialogAsync(model);
        }

        public async void ExportAllLogs()
        {
            using (System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog())
            {
                saveFile.Filter = "html file (*.html)|*.html";
                saveFile.CheckPathExists = true;
                saveFile.FileName = $"slstudio_log_{DateTime.Now.ToString().Replace('/', '-').Replace(' ', '_').Replace(':', '-')}";

                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    IsBusy = true;
                    await Task.Run(() =>
                    {
                        loggingService.ExportLogsToHtml(saveFile.FileName);
                    });
                    IsBusy = false;
                }
            }
        }

        private async Task FetchLogsAsync()
        {
            IsBusy = true;
            await Task.Run(() =>
            {
                Logs = loggingService.GetLogs();
            });
            IsBusy = false;
        }
    }
}