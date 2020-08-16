using SLStudio.Core;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.ToolBox.ViewModels;
using SLStudio.RpkEditor.Rpk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "rpkEditorName", "rpkEditorDescription", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/Icons/rpkFileIcon.png")]
    internal class RpkEditorViewModel : FileDocumentPanelBase, IRpkEditor
    {
        private readonly RpkMetadata rpk;
        private readonly BindableCollection<RpkEditorBase> editors;

        private readonly RpkCodeViewModel code;
        private readonly RpkDesignerViewModel designer;
        private readonly RpkStatsViewModel stats;

        public RpkEditorViewModel(IWindowManager windowManager, IObjectFactory objectFactory)
        {
            rpk = new RpkMetadata();
            editors = new BindableCollection<RpkEditorBase>();

            code = new RpkCodeViewModel();
            designer = new RpkDesignerViewModel(rpk, windowManager, objectFactory);
            stats = new RpkStatsViewModel();

            editors.Add(code);
            editors.Add(designer);
            editors.Add(stats);

            SelectedEditor = designer;

            ToolboxContent = new RpkToolBoxViewModel();
        }

        public IEnumerable<RpkEditorBase> Editors => editors;

        public RpkEditorBase SelectedEditor
        {
            get => GetProperty(() => SelectedEditor);
            set
            {
                if (SelectedEditor == value)
                    return;

                OnSelectedItemChanged(SelectedEditor, value).FireAndForget();
                SetProperty(() => SelectedEditor, value);
            }
        }

        private Task OnSelectedItemChanged(RpkEditorBase oldItem, RpkEditorBase newItem)
        {
            return Task.CompletedTask;
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
    }

    public interface IRpkEditor : IFileDocumentPanel
    {
    }
}