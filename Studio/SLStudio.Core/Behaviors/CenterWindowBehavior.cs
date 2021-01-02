using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.Windows;
using System.Windows.Forms;

namespace SLStudio.Core.Behaviors
{
    public class CenterWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= OnLoaded;

            var currentMonitor = Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(AssociatedObject).Handle);
            var source = PresentationSource.FromVisual(AssociatedObject);
            var dpiScaling = (source != null && source.CompositionTarget != null ? source.CompositionTarget.TransformFromDevice.M11 : 1);

            var workArea = currentMonitor.WorkingArea;
            var workAreaWidth = (int)Math.Floor(workArea.Width * dpiScaling);
            var workAreaHeight = (int)Math.Floor(workArea.Height * dpiScaling);

            AssociatedObject.Left = (((workAreaWidth - (AssociatedObject.Width * dpiScaling)) / 2) + (workArea.Left * dpiScaling));
            AssociatedObject.Top = (((workAreaHeight - (AssociatedObject.Height * dpiScaling)) / 2) + (workArea.Top * dpiScaling));
        }
    }
}