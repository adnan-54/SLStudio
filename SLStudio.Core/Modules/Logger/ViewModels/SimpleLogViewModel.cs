using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Logger.ViewModels
{
    class SimpleLogViewModel : ViewModel
    {
        private readonly ILoggingService loggingService;

        public SimpleLogViewModel(ILoggingService loggingService)
        {
            this.loggingService = loggingService;

            DisplayName = "SLStudio - Simple Log";
            
            FetchLogsAsync().FireAndForget();
        }

        public string SimpleLog
        {
            get => GetProperty(() => SimpleLog);
            set => SetProperty(() => SimpleLog, value);
        }

        public async void ExportToHtml()
        {
            using (System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog())
            {
                saveFile.Filter = "txt file (*.txt)|*.txt";
                saveFile.CheckPathExists = true;
                saveFile.FileName = $"slstudio_simplelog_{System.DateTime.Now.ToString().Replace('/', '-').Replace(' ', '_').Replace(':', '-')}";

                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        File.WriteAllText(saveFile.FileName, SimpleLog);
                    });
                }
            }
        }

        public async Task FetchLogsAsync()
        {
            await Task.Run(() =>
            {
                SimpleLog = loggingService.GetSimpleLogs();
            });
        }
    }
}
