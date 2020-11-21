using SLStudio.RpkEditor.Modules.RpkEditor.ViewModels;
using System;

namespace SLStudio.RpkEditor.Events
{
    internal class SelectedEditorChanged : EventArgs
    {
        public SelectedEditorChanged(RpkEditorBase oldEditor, RpkEditorBase newEditor)
        {
            OldEditor = oldEditor;
            NewEditor = newEditor;
        }

        public RpkEditorBase OldEditor { get; }
        public RpkEditorBase NewEditor { get; }
    }
}