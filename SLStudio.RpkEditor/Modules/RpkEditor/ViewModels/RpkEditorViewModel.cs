using SLStudio.Core;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using System;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "rpkEditorName", "rpkEditorDescription", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/rpkFileIcon.png")]
    internal class RpkEditorViewModel : DocumentPanelBase, IRpkEditor
    {
    }

    public interface IRpkEditor : IDocumentPanel
    {
    }
}