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
        private readonly StartPageViewModel startPage;

        public RecentFileViewModel(string fileName, DateTime modifiedDate, StartPageViewModel startPage, Uri iconSource)
        {
            this.startPage = startPage;
            FileName = Path.GetFileName(fileName);
            Location = fileName;
            ModifiedDate = modifiedDate;
            IconSource = iconSource;
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
            startPage.UpdatePinned(this);
        }

        public void Unpin()
        {
            IsPinned = false;
            startPage.UpdatePinned(this);
        }

        public void Remove()
        {
            startPage.Remove(this);
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