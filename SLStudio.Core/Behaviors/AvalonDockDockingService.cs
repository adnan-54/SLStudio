using AvalonDock;
using DevExpress.Mvvm.UI;
using System;
using System.Windows;

namespace SLStudio.Core.Docking
{
    internal class AvalonDockDockingService : ServiceBaseGeneric<DockingManager>
    {
        public object ActiveContent
        {
            get => GetValue(ActiveContentProperty);
            set => SetValue(ActiveContentProperty, value);
        }

        public static readonly DependencyProperty ActiveContentProperty = DependencyProperty.Register(nameof(ActiveContent), typeof(object), typeof(AvalonDockDockingService), new PropertyMetadata(OnActiveContentChanged));

        private static void OnActiveContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AvalonDockDockingService).OnActiveContentChanged(e);

        private readonly IShell shell;
        private readonly IThemeManager themeManager;

        public AvalonDockDockingService()
        {
            shell = IoC.Get<IShell>();
            themeManager = IoC.Get<IThemeManager>();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.DocumentClosing += OnDocumentClosing;
            AssociatedObject.DocumentClosed += OnDocumentClosed;
            //AssociatedObject.LayoutUpdated += OnLayoutUpdated;
            themeManager.ThemeChanged += OnThemeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateTheme();
        }

        public void OnActiveContentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IWorkspacePanel newItem)
                newItem.OnActivated();
            if (e.OldValue is IWorkspacePanel oldItem)
                oldItem.OnDeactivated();
        }

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.Document.Content is IWorkspacePanel panel)
                panel.OnClosing(e);
        }

        private void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (e.Document.Content is IWorkspacePanel panel)
            {
                panel.OnClosed(e);
                shell?.ClosePanel(panel);
            }
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            var icon = mainWindow?.Icon;
            foreach (var window in AssociatedObject.FloatingWindows)
            {
                window.Owner = null;
                window.Icon = icon;
                window.ShowInTaskbar = true;
                window.Title = "SLStudio";
            }
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            UpdateTheme();
        }

        private void UpdateTheme()
        {
            AssociatedObject.Theme = themeManager.CurrentTheme.DockTheme;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.DocumentClosing -= OnDocumentClosing;
            AssociatedObject.DocumentClosed -= OnDocumentClosed;
            //AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
            themeManager.ThemeChanged -= OnThemeChanged;
            base.OnDetaching();
        }
    }
}