using SLStudio.Core;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.ToolBox.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "rpkEditorName", "rpkEditorDescription", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/rpkFileIcon.png")]
    internal class RpkEditorViewModel : FileDocumentPanelBase, IRpkEditor
    {
        private readonly BindableCollection<RpkEditorBase> editors;

        private readonly RpkCodeViewModel code;
        private readonly RpkDesignerViewModel designer;
        private readonly RpkStatsViewModel stats;

        public RpkEditorViewModel()
        {
            editors = new BindableCollection<RpkEditorBase>();

            code = new RpkCodeViewModel();
            editors.Add(code);
            designer = new RpkDesignerViewModel();
            editors.Add(designer);
            stats = new RpkStatsViewModel();
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

                SetProperty(() => SelectedEditor, value);
                OnSelectedItemChanged();
            }
        }

        public void OnSelectedItemChanged()
        {
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