using DevExpress.Mvvm.UI.Interactivity;
using System.Windows;
using System.Windows.Input;

namespace SLStudio.Core.Behaviors
{
    public class DraggableWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDown += OnMouseDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDown -= OnMouseDown;
            base.OnDetaching();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                AssociatedObject.DragMove();
        }
    }
}