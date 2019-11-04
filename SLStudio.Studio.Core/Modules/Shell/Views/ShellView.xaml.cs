using SLStudio.Studio.Core.Framework;
using SLStudio.Studio.Core.Modules.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SLStudio.Studio.Core.Modules.Shell.Views
{
    public partial class ShellView : IShellView
    {
        public ShellView()
        {
            InitializeComponent();
        }

        public void LoadLayout(Stream stream, Action<ITool> addToolCallback, Action<IDocument> addDocumentCallback,
                               Dictionary<string, ILayoutItem> itemsState)
        {
            LayoutUtility.LoadLayout(Manager, stream, addDocumentCallback, addToolCallback, itemsState);
        }

        public void SaveLayout(Stream stream)
        {
            LayoutUtility.SaveLayout(Manager, stream);
        }

        private void OnManagerLayoutUpdated(object sender, EventArgs e)
        {
            UpdateFloatingWindows();
        }

        public void UpdateFloatingWindows()
        {
            var mainWindow = Window.GetWindow(this);
            System.Windows.Media.ImageSource mainWindowIcon = mainWindow?.Icon;
            var showFloatingWindowsInTaskbar = ((ShellViewModel)DataContext).ShowFloatingWindowsInTaskbar;
            foreach (var window in Manager.FloatingWindows)
            {
                window.Icon = mainWindowIcon;
                window.ShowInTaskbar = showFloatingWindowsInTaskbar;
            }
        }
    }
}
