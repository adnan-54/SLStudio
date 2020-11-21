using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SLStudio.Core;
using SLStudio.Logging;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Events;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.ToolBox.ViewModels;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "rpkEditorName", "rpkEditorDescription", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/Icons/rpkFileIcon.png")]
    internal class RpkEditorViewModel : FileDocumentPanelBase, IRpkEditor
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<RpkEditorViewModel>();

        private readonly RpkMetadata rpk;
        private readonly RpkManager rpkManager;
        private readonly BindableCollection<RpkEditorBase> editors;

        private readonly RpkCodeViewModel code;
        private readonly RpkDesignerViewModel designer;
        private readonly RpkStatsViewModel stats;

        private event EventHandler<SelectedEditorChanged> SelectedEditorChanged;

        public RpkEditorViewModel(IWindowManager windowManager, IObjectFactory objectFactory, IUiSynchronization uiSynchronization)
        {
            rpk = new RpkMetadata();
            rpkManager = new RpkManager(rpk, objectFactory, windowManager);
            editors = new BindableCollection<RpkEditorBase>();

            code = new RpkCodeViewModel();
            designer = new RpkDesignerViewModel(rpk, rpkManager);
            stats = new RpkStatsViewModel();

            editors.Add(code);
            editors.Add(designer);
            editors.Add(stats);

            SelectedEditor = designer;

            SelectedEditorChanged += OnSelectedEditorChanged;

            ToolboxContent = new RpkToolBoxViewModel(this, rpkManager);
        }

        public IEnumerable<RpkEditorBase> Editors => editors;

        public RpkEditorBase SelectedEditor
        {
            get => GetProperty(() => SelectedEditor);
            set
            {
                if (SelectedEditor == value)
                    return;

                var oldEditor = SelectedEditor;

                SetProperty(() => SelectedEditor, value);
                SelectedEditorChanged?.Invoke(this, new SelectedEditorChanged(oldEditor, value));
            }
        }

        public bool IsBusy
        {
            get => GetProperty(() => IsBusy);
            set => SetProperty(() => IsBusy, value);
        }

        protected override Task DoLoad()
        {
            return Task.CompletedTask;
        }

        protected override Task DoNew(string content)
        {
            return Task.CompletedTask;
        }

        protected override Task DoSave()
        {
            return Task.CompletedTask;
        }

        private void OnSelectedEditorChanged(object sender, SelectedEditorChanged e)
        {
            if (e.NewEditor == code && e.OldEditor == designer)
            {
                try
                {
                    IsBusy = true;
                    var content = rpkManager.Parse();
                    code.UpdateContent(content);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    SelectedEditor = e.OldEditor;
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }

    public interface IRpkEditor : IFileDocumentPanel
    {
    }
}