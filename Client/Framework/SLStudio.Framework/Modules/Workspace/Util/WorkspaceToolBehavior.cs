using AvalonDock.Layout;
using DevExpress.Mvvm.UI.Interactivity;

namespace SLStudio
{
    internal class WorkspaceToolBehavior : Behavior<LayoutAnchorable>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += AssociatedObject_Closing;
        }

        private void AssociatedObject_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class WorkspaceDocumentBehavior : Behavior<LayoutDocument>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += AssociatedObject_Closing;
        }

        private void AssociatedObject_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
