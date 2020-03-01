﻿using DevExpress.Mvvm.UI.Interactivity;
using MahApps.Metro.Controls;
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

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Closing -= OnClosing;
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (Settings.Default.ShowConsoleAtStartup)
            //IoC.Get<IWindowManager>().ShowWindowAsync(IoC.Get<IConsole>());
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}