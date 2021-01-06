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
        private IWorkspaceItem lastActiveWorkspace;
        private IWorkspaceItem currentActiveWorkspace;
        private IToolItem lastActiveTool;
        private IToolItem currentActiveTool;
        private IDocumentItem lastActiveDocument;
        private IDocumentItem currentActiveDocument;
        private IDocumentItem documentToClose;

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
            if (AssociatedObject.ActiveContent is not IWorkspaceItem workspaceItem)
                return;

            lastActiveWorkspace = currentActiveWorkspace;
            currentActiveWorkspace = workspaceItem;

            messenger.Send(new ActiveWorkspaceChangedEvent(lastActiveWorkspace, currentActiveWorkspace));

            if (workspaceItem is IToolItem toolItem)
            {
                lastActiveTool = currentActiveTool;
                currentActiveTool = toolItem;

                messenger.Send(new ActiveToolChangedEvent(lastActiveTool, currentActiveTool));
            }
            else
            if (workspaceItem is IDocumentItem documentItem)
            {
                lastActiveDocument = currentActiveDocument;
                currentActiveDocument = documentItem;

                messenger.Send(new ActiveDocumentChangedEvent(lastActiveDocument, currentActiveDocument));
            }
        }

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.Document.Content is not IDocumentItem document)
                return;

            var result = document.OnClosing();
            if (result)
            {
                documentToClose = document;
            }

            e.Cancel = !result;
        }

        private async void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (documentToClose == null)
                return;

            await shell.CloseWorkspaces(documentToClose);
            documentToClose.OnClosed();

            if (documentToClose == currentActiveDocument)
                FocusLastDocument();

            DisposeDocumentToClose();
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

        private void DisposeDocumentToClose()
        {
            if (documentToClose == lastActiveDocument)
                lastActiveDocument = null;
            if (documentToClose == currentActiveDocument)
                currentActiveDocument = null;
            if (documentToClose == lastActiveWorkspace)
                lastActiveWorkspace = null;
            if (documentToClose == currentActiveWorkspace)
                currentActiveWorkspace = null;

            documentToClose.Dispose();
            documentToClose = null;
        }

        private void FocusLastDocument()
        {
            var target = lastActiveDocument ?? shell.Workspaces.OfType<IDocumentItem>().LastOrDefault();
            if (target != null)
                shell.OpenWorkspaces(target);
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