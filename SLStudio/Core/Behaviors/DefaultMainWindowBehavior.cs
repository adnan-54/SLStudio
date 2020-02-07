using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace SLStudio.Core.Behaviors
{
    public class DefaultMainWindowBehavior : Behavior<MetroWindow>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += OnClosing;
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Closing -= OnClosing;
            base.OnDetaching();
        }
    }
}
