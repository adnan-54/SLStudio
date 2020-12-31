using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.ToolBox.ViewModels;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    class RpkEditorToolProvider : ToolContentProvider
    {
        private readonly RpkEditorViewModel rpkEditor;
        private readonly RpkManager rpkManager;

        public RpkEditorToolProvider(RpkEditorViewModel rpkEditor, RpkManager rpkManager)
        {
            this.rpkEditor = rpkEditor;
            this.rpkManager = rpkManager;
        }

        public override void Register()
        {
            Register<IToolbox, RpkToolBoxViewModel>(new RpkToolBoxViewModel(rpkEditor, rpkManager));
        }
    }
}
