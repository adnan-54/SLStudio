using AvalonDock;
using DevExpress.Mvvm;
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
        private readonly IMessenger messenger;
        private IDocumentPanel documentToClose;

        public AvalonDockDockingService()
        {
            shell = IoC.Get<IShell>();
            themeManager = IoC.Get<IThemeManager>();
            messenger = IoC.Get<IMessenger>();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.DocumentClosing += OnDocumentClosing;
            AssociatedObject.DocumentClosed += OnDocumentClosed;
            AssociatedObject.LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Theme = themeManager.CurrentTheme.DockTheme;
        }

        public void OnActiveContentChanged(DependencyPropertyChangedEventArgs e)
        {
            var newItem = e.NewValue as IPanelItem;
            var oldItem = e.OldValue as IPanelItem;

            newItem?.OnActivated();
            oldItem?.OnDeactivated();

            messenger.Send(new AvalonDockActiveContentChangedEventArgs(newItem));
        }

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            var panel = e.Document.Content as IDocumentPanel;

            panel?.OnClosing(e);

            if (!e.Cancel)
                documentToClose = panel;
        }

        private void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (documentToClose != null)
            {
                documentToClose.OnClosed(e);
                shell?.ClosePanel(documentToClose);
                documentToClose = null;
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
                window.Title = $"SLStudio - {window.Model.Root.ActiveContent?.Title}";
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.DocumentClosing -= OnDocumentClosing;
            AssociatedObject.DocumentClosed -= OnDocumentClosed;
            AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
            base.OnDetaching();
        }
    }
}