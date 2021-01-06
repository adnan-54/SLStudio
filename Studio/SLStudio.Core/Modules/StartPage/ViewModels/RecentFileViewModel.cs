using DevExpress.Mvvm;
using SLStudio.Core.Modules.StartPage.Resources;
using SLStudio.Logging;
using System;
using System.IO;
using System.Windows;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class RecentFileViewModel : BindableBase
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<RecentFileViewModel>();

        public RecentFileViewModel(string fileName, DateTime modifiedDate)
        {
            FileName = Path.GetFileName(fileName);
            Location = fileName;
            ModifiedDate = modifiedDate;
        }

        public Uri IconSource { get; set; }
        public string FileName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Location { get; set; }
        public string ToolTip => $"{StartPageResources.OpenLocalSolution}{Environment.NewLine}{Location}";

        public bool IsPinned
        {
            get => GetProperty(() => IsPinned);
            set => SetProperty(() => IsPinned, value);
        }

        public void Pin()
        {
            IsPinned = true;
        }

        public void Unpin()
        {
            IsPinned = false;
        }

        public void CopyPath()
        {
            try
            {
                Clipboard.SetText(Location);
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
            }
        }
    }
}