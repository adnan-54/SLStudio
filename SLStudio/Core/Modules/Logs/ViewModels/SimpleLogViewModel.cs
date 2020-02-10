using Caliburn.Micro;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Logs.ViewModels
{
    internal class SimpleLogViewModel : Screen
    {
        private readonly ILoggingService loggingService;

        private string log;

        public SimpleLogViewModel(ILoggingService loggingService)
        {
            this.loggingService = loggingService;

            FetchLogAsync().FireAndForget();

            DisplayName = "Log";
        }

        public string Log
        {
            get => log;
            set
            {
                log = value;
                NotifyOfPropertyChange(() => Log);
            }
        }

        public async Task ExportToTextFile()
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
                        File.WriteAllText(saveFile.FileName, Log);
                    });
                }
            }
        }

        public async Task FetchLogAsync()
        {
            await Task.Run(() =>
            {
                log = loggingService.GetSimpleLogs();
            });
        }
    }
}
