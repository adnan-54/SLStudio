using Humanizer;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using SLStudio.Core.Modules.LogsVisualizer.Resources;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        private readonly ILogManager logManager;
        private int busyOperations;

        public LogVisualizerViewModel(IDialogService dialogService, IWindowManager windowManager, ILogManager logManager)
        {
            this.dialogService = dialogService;
            this.windowManager = windowManager;
            this.logManager = logManager;

            DisplayName = LogsVisualizerResources.window_title_logs;
        }

        public bool IsBusy => busyOperations > 0;

        public IEnumerable<Log> Logs
        {
            get => GetProperty(() => Logs);
            set
            {
                SetProperty(() => Logs, value);

                RaisePropertyChanged(() => TotalLogs);
                RaisePropertyChanged(() => TotalSize);
            }
        }

        public Log SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        public string StatusBarStatus
        {
            get => GetProperty(() => StatusBarStatus);
            set => SetProperty(() => StatusBarStatus, value);
        }

        public string TotalLogs => string.Format(LogsVisualizerResources.statusbar_totalLogsFormat, Logs?.Count());

        public string TotalSize => string.Format(LogsVisualizerResources.statusbar_totalSizeFormat, logManager.GetLogsSize().Bytes().Humanize("MB"));

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
            var vm = new SimpleLogVisualizerViewModel(dialogService, logManager);
            windowManager.ShowDialog(vm);
        }

        public void ClearAll()
        {
            if (!Logs.Any())
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

                logManager.DeleteAllLogs();
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

        private Task FetchLogs()
        {
            Busy();

            Logs = logManager.GetLogs();

            Idle();

            return Task.CompletedTask;
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

        private Task<string> BuildHtmlString()
        {
            return Task.Run(() =>
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SLStudio.Core.Modules.LogsVisualizer.Resources.htmlTemplate.txt");
                using StreamReader reader = new StreamReader(stream);

                return reader.ReadToEnd().Replace("****tablePlaceHolder****", BuildRows());
            });
        }

        private string BuildRows()
        {
            var sb = new StringBuilder();
            var logs = new List<Log>(Logs.Take(1000));

            foreach (var log in logs)
            {
                sb.AppendLine("\t\t\t<tr>");
                sb.AppendLine($"\t\t\t\t<td>{log.Id}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.Sender}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.Level}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.Title}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.Message}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.Date}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.CallerFile}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.CallerMember}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.CallerLine}</td>");
                sb.AppendLine($"\t\t\t\t<td>{log.StackTrace}</td>");
                sb.AppendLine("\t\t\t</tr>");
            }

            return sb.ToString();
        }
    }
}