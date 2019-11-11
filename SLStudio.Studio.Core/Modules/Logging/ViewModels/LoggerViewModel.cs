using SLStudio.Studio.Core.Framework;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Logging.ViewModels
{
    [Export(typeof(LoggerViewModel))]
    public class LoggerViewModel : WindowBase
    {
        public LoggerViewModel()
        {
            DisplayName = "Logs";
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;

                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private DataTable logs;
        public DataTable Logs
        {
            get => logs;
            set
            {
                logs = value;

                NotifyOfPropertyChange(() => Logs);
            }
        }

        public async Task Export()
        {
            IsBusy = true;

            try
            {
                await LogManager.ExportLogToHtml();
            }
            catch { }
            finally
            {
                IsBusy = false;
            }
        }

        public void Clear()
        {
            LogManager.ClearLog();
            Load();
        }

        public void Ok()
        {
            TryClose();
        }

        private void Load()
        {
            IsBusy = true;
            
            try
            {
                Logs = LogManager.GetLogs();
            }
            catch { }
            finally
            {
                IsBusy = false;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Load();
        }
    }
}
