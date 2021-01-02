using DevExpress.Mvvm;
using SLStudio.Core.Modules.StartPage.Resources;
using SLStudio.Logging;
using System;
using System.Windows;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class RecentFileViewModel : BindableBase
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<RecentFileViewModel>();

        private StartPageViewModel parent;

        public RecentFileViewModel(StartPageViewModel parent)
        {
            this.parent = parent;
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

        public void Remove()
        {
            parent.RemoveItem(this);
            parent = null;
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
                logger.Error(ex);
            }
        }
    }
}