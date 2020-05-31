using DevExpress.Mvvm.UI.Interactivity;
using MahApps.Metro.Controls;
using System.Windows;

namespace SLStudio.Core.Behaviors
{
    public class DefaultMainWindowBehavior : Behavior<MetroWindow>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            SetupShutdownMode();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (Settings.Default.ShowConsoleAtStartup)
            //IoC.Get<IWindowManager>().ShowWindow<IConsole>();
        }

        private void SetupShutdownMode()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = AssociatedObject;
        }
    }
}