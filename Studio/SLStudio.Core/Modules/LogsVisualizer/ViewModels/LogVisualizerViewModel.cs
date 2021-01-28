using Humanizer;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using SLStudio.Core.Modules.LogsVisualizer.Resources;
using SLStudio.Logging;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core.Modules.LogsVisualizer.ViewModels
{
    internal class LogVisualizerViewModel : WindowViewModel, ILogVisualizer
    {
        private readonly IDialogService dialogService;
        private readonly IWindowManager windowManager;

        private int busyOperations;

        public LogVisualizerViewModel(IDialogService dialogService, IWindowManager windowManager)
        {
            this.dialogService = dialogService;
            this.windowManager = windowManager;
            DisplayName = LogsVisualizerResources.window_title_logs;
        }

        public bool IsBusy => busyOperations > 0;

        public DataTable Logs
        {
            get => GetProperty(() => Logs);
            set
            {
                SetProperty(() => Logs, value);

                RaisePropertyChanged(() => TotalLogs);
                RaisePropertyChanged(() => TotalSize);
            }
        }

        public DataRowView SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        public string StatusBarStatus
        {
            get => GetProperty(() => StatusBarStatus);
            set => SetProperty(() => StatusBarStatus, value);
        }

        public string TotalLogs => string.Format(LogsVisualizerResources.statusbar_totalLogsFormat, Logs?.Rows?.Count);

        public static string TotalSize => string.Format(LogsVisualizerResources.statusbar_totalSizeFormat, LogManager.GetLogFileSize().Bytes().Humanize("MB"));

        public void Refresh()
        {
            FetchLogs().FireAndForget();
        }

        public async Task ExportToHtml()
        {
            var settings = new SaveFileDialogSettings()
            {
                AddExtension = true,
                DefaultExt = ".html",
                FileName = $"SLStudio Logs {DateTime.Now.ToString().ToFileName()}",
                Filter = LogsVisualizerResources.filter_htmlFile,
                Title = LogsVisualizerResources.dialog_title_exportHtml,
                ValidateNames = true
            };

            var result = dialogService.ShowSaveFileDialog(this, settings);
            if (result == true)
            {
                Busy();

                var content = await BuildHtmlString();
                await File.WriteAllTextAsync(settings.FileName, content);

                Idle();
            }
        }

        public void ViewSimpleLog()
        {
            var vm = new SimpleLogVisualizerViewModel(dialogService);
            windowManager.ShowDialog(vm);
        }

        public async Task ClearAll()
        {
            if (Logs?.Rows?.Count <= 1)
                return;

            var settings = new MessageBoxSettings()
            {
                Button = MessageBoxButton.YesNo,
                DefaultResult = MessageBoxResult.No,
                Icon = MessageBoxImage.Question,
                Caption = LogsVisualizerResources.dialog_title_clearLogs,
                MessageBoxText = LogsVisualizerResources.dialog_message_clearConfirmation
            };
            var result = dialogService.ShowMessageBox(this, settings);
            if (result == MessageBoxResult.Yes)
            {
                Busy();

                await LogManager.ClearAll();
                Refresh();

                Idle();
            }
        }

        public void ViewDetails()
        {
            if (SelectedItem == null)
                return;

            var vm = new LogsDetailsViewModel(SelectedItem);
            windowManager.ShowDialog(vm);
        }

        protected override void OnLoaded()
        {
            Refresh();
        }

        private async Task FetchLogs()
        {
            Busy();

            Logs = await LogManager.GetLogs();

            Idle();
        }

        private void Busy()
        {
            if (++busyOperations > 0)
            {
                RaisePropertyChanged(() => IsBusy);
                StatusBarStatus = LogsVisualizerResources.statusbar_status_working;
            }
        }

        private void Idle()
        {
            if (--busyOperations <= 0)
            {
                RaisePropertyChanged(() => IsBusy);
                StatusBarStatus = LogsVisualizerResources.statusbar_status_ready;
            }
        }

        private async Task<string> BuildHtmlString()
        {
            var result = string.Empty;

            await Task.Run(() =>
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SLStudio.Core.Modules.LogsVisualizer.Resources.htmlTemplate.txt");
                using StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();

                result = result.Replace("****tablePlaceHolder****", BuildRows());
            });

            return result;
        }

        private string BuildRows()
        {
            var sb = new StringBuilder();

            for (int i = 0; i <= 1000; i++)
            {
                if (i >= Logs.Rows.Count - 1)
                    break;

                sb.AppendLine("\t\t\t<tr>");
                sb.AppendLine($"\t\t\t\t<td>{Logs.Rows[i][0]}</td>");
                sb.AppendLine($"\t\t\t\t<td>{Logs.Rows[i][1]}</td>");
                sb.AppendLine($"\t\t\t\t<td>{Logs.Rows[i][2]}</td>");
                sb.AppendLine($"\t\t\t\t<td>{Logs.Rows[i][3]}</td>");
                sb.AppendLine($"\t\t\t\t<td>{Logs.Rows[i][4]}</td>");
                sb.AppendLine($"\t\t\t\t<td>{Logs.Rows[i][5]}</td>");
                sb.AppendLine("\t\t\t</tr>");
            }

            return sb.ToString();
        }
    }
}