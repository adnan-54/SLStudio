using SLStudio.Core.Logging;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core.Modules.Logger.ViewModels
{
    internal class LogsViewModel : ViewModel, ILogsView
    {
        private readonly IWindowManager windowManager;
        private readonly ILoggingService loggingService;

        public LogsViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            loggingService = LogManager.LoggingService;
            FetchLogsAsync().FireAndForget();

            DisplayName = "SLStudio - Logs";
        }

        public bool IsBusy
        {
            get => GetProperty(() => IsBusy);
            set
            {
                SetProperty(() => IsBusy, value);
                if (IsBusy)
                    StatusText = Resources.LogsResources.Working;
                else
                    StatusText = Resources.LogsResources.Ready;
            }
        }

        public string StatusText
        {
            get => GetProperty(() => StatusText);
            set => SetProperty(() => StatusText, value);
        }

        public DataTable Logs
        {
            get => GetProperty(() => Logs);
            set
            {
                SetProperty(() => Logs, value);
                RaisePropertyChanged(() => CanExport);
                RaisePropertyChanged(() => CanClear);
                RaisePropertyChanged(() => CanViewSimpleLog);
            }
        }

        public object SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        public bool CanExport
            => Logs?.Rows.Count > 0;

        public bool CanClear
            => Logs?.Rows.Count > 0;

        public bool CanViewSimpleLog
            => loggingService.SimpleLogFileExists;

        private Task FetchLogsAsync()
        {
            return Task.Run(() =>
            {
                IsBusy = true;
                Logs = loggingService.GetLogs();
                if (Logs.Rows.Count > 0)
                    SelectedItem = Logs.Rows[0];
                else
                    SelectedItem = null;

                IsBusy = false;
            });
        }

        public async void FetchLogs()
        {
            await FetchLogsAsync();
            StatusText = Resources.LogsResources.LogsUpdatedSuccessfully;
        }

        public async void ExportToHtml()
        {
            if (!CanExport)
                return;

            using (System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog())
            {
                saveFile.Filter = "html file (*.html)|*.html";
                saveFile.CheckPathExists = true;
                saveFile.FileName = $"slstudio_log_{DateTime.Now.ToString().Replace('/', '-').Replace(' ', '_').Replace(':', '-')}";

                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    IsBusy = true;
                    await Task.Run(() => { loggingService.ExportLogsToHtml(saveFile.FileName); });
                    IsBusy = false;
                    StatusText = Resources.LogsResources.LogsExportedSuccessfully;
                }
            }
        }

        public async void ClearAll()
        {
            var result = MessageBox.Show(Resources.LogsResources.AreYouSure, Resources.LogsResources.ClearAll, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                IsBusy = true;
                await Task.Run(() => loggingService.ClearAllLogs());
                FetchLogs();
                IsBusy = false;
                StatusText = Resources.LogsResources.AllLogsClearedSuccessfully;
            }
        }

        public void ViewSimpleLog()
        {
            var simpleLogView = new SimpleLogViewModel();
            windowManager.ShowDialog(simpleLogView);
        }

        public void ViewLogDetails()
        {
            if (SelectedItem is DataRowView row)
            {
                var detailsView = new LogDetailsViewModel(row);
                windowManager.ShowDialog(detailsView);
            }
        }
    }

    internal interface ILogsView
    {
    }
}