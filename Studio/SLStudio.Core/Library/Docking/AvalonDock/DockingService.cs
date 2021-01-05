using AvalonDock;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using System;
using System.Linq;
using System.Windows;

namespace SLStudio.Core.Docking
{
    internal class DockingService : ServiceBaseGeneric<DockingManager>
    {
        private readonly IShell shell;
        private readonly IThemeManager themeManager;
        private readonly IMessenger messenger;
        private IDocumentItem documentToClose;
        private IDocumentItem documentToFocus;
        private IWorkspaceItem previousActiveItem;
        private IWorkspaceItem currentActiveItem;
        private IDocumentItem lastActiveDocument;
        private IDocumentItem currentActiveDocument;

        public DockingService()
        {
            shell = IoC.Get<IShell>();
            themeManager = IoC.Get<IThemeManager>();
            messenger = IoC.Get<IMessenger>();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.ActiveContentChanged += OnActiveContentChanged;
            AssociatedObject.DocumentClosing += OnDocumentClosing;
            AssociatedObject.DocumentClosed += OnDocumentClosed;
            AssociatedObject.LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Theme = themeManager.CurrentTheme.DockTheme;
        }

        private void OnActiveContentChanged(object sender, EventArgs e)
        {
            previousActiveItem = currentActiveItem;
            currentActiveItem = AssociatedObject.ActiveContent as IWorkspaceItem;

            if (AssociatedObject.ActiveContent is IDocumentItem document)
            {
                lastActiveDocument = currentActiveDocument;
                currentActiveDocument = document;
            }

            previousActiveItem?.OnDeactivated();
            currentActiveItem?.OnActivated();

            messenger.Send(new ActiveWorkspaceChangedEvent(currentActiveItem));
        }

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.Document.Content is not IDocumentItem document)
                return;

            var result = document.OnClosing();
            if (result)
            {
                documentToClose = document;
                documentToFocus = lastActiveDocument;
            }

            e.Cancel = !result;
        }

        private async void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (documentToClose == null)
                return;

            documentToClose.OnClosed();
            await shell.CloseWorkspaces(documentToClose);
            documentToClose.Dispose();

            if (documentToFocus != null)
            {
                if (shell.Workspaces.Contains(documentToFocus))
                    await shell.OpenWorkspaces(documentToFocus);
                else
                    await shell.OpenWorkspaces(shell.Workspaces.OfType<IDocumentItem>().LastOrDefault());
            }

            DisposeItem(documentToClose);

            documentToClose = null;
            documentToFocus = null;
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

        private void DisposeItem(IWorkspaceItem item)
        {
            if (item == documentToClose)
                documentToClose = null;

            if (item == documentToFocus)
                documentToFocus = null;

            if (item == previousActiveItem)
                previousActiveItem = null;

            if (item == currentActiveItem)
                currentActiveItem = null;

            if (item == lastActiveDocument)
                lastActiveDocument = null;

            if (item == currentActiveDocument)
                currentActiveDocument = null;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.ActiveContentChanged -= OnActiveContentChanged;
            AssociatedObject.DocumentClosing -= OnDocumentClosing;
            AssociatedObject.DocumentClosed -= OnDocumentClosed;
            AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
            base.OnDetaching();
        }
    }
}