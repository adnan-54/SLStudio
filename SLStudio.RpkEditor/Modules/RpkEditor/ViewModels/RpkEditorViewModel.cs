using SLStudio.Core;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using SLStudio.RpkEditor.Modules.ToolBox.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "rpkEditorName", "rpkEditorDescription", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/rpkFileIcon.png")]
    internal class RpkEditorViewModel : FileDocumentPanelBase, IRpkEditor
    {
        public RpkEditorViewModel()
        {
            ToolboxContent = new RpkToolBoxViewModel();
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