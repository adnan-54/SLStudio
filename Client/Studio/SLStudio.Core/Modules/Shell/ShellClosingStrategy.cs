using SLStudio.Core.Docking;
using System;
using System.ComponentModel;
using System.Linq;

namespace SLStudio.Core
{
    internal class ShellOpeningStrategy : IShellOpeningStrategy
    {
        private readonly ICommandLineArguments commandLineArguments;
        private readonly IFileService fileService;

        public ShellOpeningStrategy(IShell shell, ICommandLineArguments commandLineArguments, IFileService fileService)
        {
            this.commandLineArguments = commandLineArguments;
            this.fileService = fileService;
            shell.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, EventArgs e)
        {
            var filesToOpen = commandLineArguments.Files;
            IFileDocumentItem lastOpenedFile = null;

            foreach (var file in filesToOpen)
            {
                if (fileService.CanHandle(file))
                    lastOpenedFile = await fileService.Open(file);
            }

            lastOpenedFile?.Activate();
        }
    }

    public interface IShellOpeningStrategy
    {
    }

    internal class ShellClosingStrategy : IShellClosingStrategy
    {
        private readonly IShell shell;
        private readonly IWindowManager windowManager;
        private readonly IFileService fileService;

        public ShellClosingStrategy(IShell shell, IWindowManager windowManager, IFileService fileService)
        {
            this.shell = shell;
            this.windowManager = windowManager;
            this.fileService = fileService;

            shell.Closing += OnClosing;
        }

        public IDockingService DockingService => shell.ServiceContainer?.GetService<IDockingService>();

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var filesToSave = shell.Workspaces.OfType<IFileDocumentItem>().Where(d => d.IsDirty).ToArray();
            if (!filesToSave.Any())
                return;

            var model = new ConfirmDocumentsClosingViewModel(filesToSave);
            windowManager.ShowDialog(model);

            if (model.Result == ConfirmDocumentsClosingViewModel.ConfirmationResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            foreach (var file in model.FilesToSave)
                fileService.Save(file);
        }
    }

    public interface IShellClosingStrategy
    {
    }
}