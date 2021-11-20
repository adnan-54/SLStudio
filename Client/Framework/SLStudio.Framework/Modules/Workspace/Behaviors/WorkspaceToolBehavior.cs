using System.ComponentModel;
using AvalonDock.Layout;
using DevExpress.Mvvm.UI.Interactivity;

namespace SLStudio
{
    internal class WorkspaceToolBehavior : Behavior<LayoutAnchorable>
    {
        private IWorkspaceTool tool;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject.Content is IWorkspaceTool workspaceTool)
                tool = workspaceTool;

            AssociatedObject.Hiding += OnHiding;
        }

        private void OnHiding(object sender, CancelEventArgs e)
        {
            tool?.OnHiding(e);
        }

        protected override void OnDetaching()
        {
            tool = null;
            AssociatedObject.Hiding -= OnHiding;

            base.OnDetaching();
        }
    }
}
