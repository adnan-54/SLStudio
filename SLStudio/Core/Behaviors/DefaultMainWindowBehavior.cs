using Caliburn.Micro;
using MahApps.Metro.Controls;
using Microsoft.Xaml.Behaviors;
using SLStudio.Properties;
using System;
using System.Windows;

namespace SLStudio.Core.Behaviors
{
    public class DefaultMainWindowBehavior : Behavior<MetroWindow>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Closing += OnClosing;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.ShowConsoleAtStartup)
                IoC.Get<IWindowManager>().ShowWindowAsync(IoC.Get<IConsole>());
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Closing -= OnClosing;
            base.OnDetaching();
        }
    }
}