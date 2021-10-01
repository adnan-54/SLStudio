using SLStudio.Logging;
using System;
using System.Windows;

namespace SLStudio.Core
{
    public abstract class DocumentBase : WorkspaceItem, IDocumentItem
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DocumentBase>();

        public IToolContentProvider ToolContentProvider { get; set; }

        public sealed override WorkspaceItemPlacement Placement => WorkspaceItemPlacement.Document;

        public sealed override ClosingBehavior ClosingBehavior => ClosingBehavior.Remove;

        public virtual bool CanCopyFileName
        {
            get => !string.IsNullOrEmpty(DisplayName);
        }

        public virtual void CopyFileName()
        {
            try
            {
                Clipboard.SetText(DisplayName);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }

    public interface IDocumentItem : IWorkspaceItem
    {
        IToolContentProvider ToolContentProvider { get; }
    }
}