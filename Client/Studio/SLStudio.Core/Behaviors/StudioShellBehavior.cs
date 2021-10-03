using DevExpress.Mvvm.UI.Interactivity;
using System.Windows;

namespace SLStudio.Core.Behaviors
{
    internal class StudioShellBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = AssociatedObject;
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            AssociatedObject.Focus();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }
    }
}