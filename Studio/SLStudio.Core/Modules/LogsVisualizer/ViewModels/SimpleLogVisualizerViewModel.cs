using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using SLStudio.Core.Modules.LogsVisualizer.Resources;
using SLStudio.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.LogsVisualizer.ViewModels
{
    internal class SimpleLogVisualizerViewModel : WindowViewModel
    {
        private readonly IDialogService dialogService;
        private readonly ILogManager logManager;

        public SimpleLogVisualizerViewModel(IDialogService dialogService, ILogManager logManager)
        {
            this.dialogService = dialogService;
            this.logManager = logManager;
            FetchLogs().FireAndForget();

            DisplayName = LogsVisualizerResources.window_title_SimpleLog;
        }

        public string SimpleLogs
        {
            get => GetProperty(() => SimpleLogs);
            set
            {
                SetProperty(() => SimpleLogs, value);
                RaisePropertyChanged(() => CanExport);
            }
        }

        public bool CanExport => !string.IsNullOrEmpty(SimpleLogs);

        public async Task Export()
        {
            if (!CanExport)
                return;

            var settings = new SaveFileDialogSettings()
            {
                AddExtension = true,
                DefaultExt = ".txt",
                FileName = $"SLStudio Simple Logs {DateTime.Now.ToString().ToFileName()}",
                Filter = LogsVisualizerResources.filter_textFile,
                Title = LogsVisualizerResources.dialog_title_exportTxt,
                ValidateNames = true
            };

            var result = dialogService.ShowSaveFileDialog(this, settings);
            if (result == true)
                await File.WriteAllTextAsync(settings.FileName, SimpleLogs);
        }

        private async Task FetchLogs()
        {
            SimpleLogs = await logManager.GetInternalLogs();
        }
    }
}