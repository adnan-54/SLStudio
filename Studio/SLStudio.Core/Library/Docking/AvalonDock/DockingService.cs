using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Layout;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SLStudio.Core.Docking
{
    internal class DockingService : ServiceBaseGeneric<DockingManager>, IDockingService
    {
        private readonly IShell shell;
        private readonly IWindowManager windowManager;
        private readonly IThemeManager themeManager;
        private readonly IFileService fileService;
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
            windowManager = IoC.Get<IWindowManager>();
            themeManager = IoC.Get<IThemeManager>();
            fileService = IoC.Get<IFileService>();
            messenger = IoC.Get<IMessenger>();
        }

        public IShell Shell { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.ActiveContentChanged += OnActiveContentChanged;
            AssociatedObject.DocumentClosing += OnDocumentClosing;
            AssociatedObject.DocumentClosed += OnDocumentClosed;
            AssociatedObject.LayoutUpdated += OnLayoutUpdated;
        }

        public void CloseFromId(string id)
        {
            var layoutPanel = AssociatedObject.Layout.Children.OfType<LayoutPanel>()?.FirstOrDefault();
            var layoutDocument = layoutPanel.Descendents().OfType<LayoutDocument>().FirstOrDefault(d => d.ContentId == id);
            layoutDocument?.Close();
        }

        public void FocusItem(IWorkspaceItem item)
        {
            if (item == null)
                item = lastActiveDocument;

            if (item == null)
                return;

            Keyboard.ClearFocus();
            Application.Current.MainWindow.Focus();
            FocusManager.SetFocusedElement(Application.Current.MainWindow, Application.Current.MainWindow);
            Keyboard.Focus(Application.Current.MainWindow);
            item.Activate();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Theme = themeManager.CurrentTheme.DockTheme;
        }

        private void OnActiveContentChanged(object sender, EventArgs e)
        {
            if (AssociatedObject.ActiveContent is not IWorkspaceItem workspaceItem)
                return;

            if (workspaceItem != currentActiveWorkspace)
            {
                lastActiveWorkspace = currentActiveWorkspace;
                currentActiveWorkspace = workspaceItem;

                messenger.Send(new ActiveWorkspaceChangedEvent(lastActiveWorkspace, currentActiveWorkspace));
            }

            if (workspaceItem is IToolItem toolItem)
            {
                if (toolItem != currentActiveTool)
                {
                    lastActiveTool = currentActiveTool;
                    currentActiveTool = toolItem;

                    messenger.Send(new ActiveToolChangedEvent(lastActiveTool, currentActiveTool));
                }
            }
            else
            if (workspaceItem is IDocumentItem documentItem)
            {
                if (documentItem != currentActiveDocument)
                {
                    lastActiveDocument = currentActiveDocument;
                    currentActiveDocument = documentItem;

                    messenger.Send(new ActiveDocumentChangedEvent(lastActiveDocument, currentActiveDocument));
                }
            }
        }

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.Document.Content is not IDocumentItem document)
                return;
            var cancelEventArgs = new CancelEventArgs();
            document.OnClosing(cancelEventArgs);

            if (document is IFileDocumentItem file && file.IsDirty)
            {
                var model = new ConfirmDocumentsClosingViewModel(file);
                windowManager.ShowDialog(model);

                if (model.Result != ConfirmDocumentsClosingViewModel.ConfirmationResult.Cancel)
                {
                    foreach (var fileToSave in model.FilesToSave)
                        fileService.Save(fileToSave);
                }
                else
                    cancelEventArgs.Cancel = true;
            }

            e.Cancel = cancelEventArgs.Cancel;

            if (!e.Cancel)
                documentToClose = document;
        }

        private async void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (documentToClose == null)
                return;

            await shell.CloseWorkspaces(documentToClose);
            documentToClose.OnClosed();

            if (documentToClose == currentActiveDocument)
                FocusLastDocument().FireAndForget();

            messenger.Send(new WorkspaceClosedEvent(documentToClose));

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

        private async Task FocusLastDocument()
        {
            var target = lastActiveDocument ?? shell.Workspaces.OfType<IDocumentItem>().LastOrDefault();
            if (target != null)
                await shell.OpenWorkspaces(target);
        }

        //todo: way to supress this from shell
        private bool ShowClosingDialog(IFileDocumentItem fileDocument)
        {
            if (fileDocument == null || !fileDocument.IsDirty)
                return true;

            //todo: show dialog
            bool? dialogResult = null;

            if (dialogResult == null)
                return false;
            else
            if (dialogResult == true)
                fileService.Save(fileDocument);

            return true;
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

    internal interface IDockingService
    {
        void CloseFromId(string id);

        void FocusItem(IWorkspaceItem item = null);
    }
}