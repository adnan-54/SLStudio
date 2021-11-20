using AvalonDock.Layout;
using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.ComponentModel;

namespace SLStudio
{
    internal class WorkspaceDocumentBehavior : Behavior<LayoutDocument>
    {
        private IWorkspaceDocument document;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject.Content is IWorkspaceDocument workspaceDocument)
                document = workspaceDocument;

            AssociatedObject.Closing += OnClosing;
            AssociatedObject.Closed += OnClosed;
        }

        protected override void OnDetaching()
        {
            document = null;
            AssociatedObject.Closing -= OnClosing;
            AssociatedObject.Closed -= OnClosed;

            base.OnDetaching();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            document?.OnClosing(e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            document?.OnClosed();
        }
    }
}
