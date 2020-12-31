using SLStudio.RpkEditor.Modules.RpkEditor.ViewModels;
using System;

namespace SLStudio.RpkEditor.Events
{
    internal class SelectedEditorChangedEventArgs : EventArgs
    {
        public SelectedEditorChangedEventArgs(RpkEditorBase oldEditor, RpkEditorBase newEditor)
        {
            OldEditor = oldEditor;
            NewEditor = newEditor;
        }

        public RpkEditorBase OldEditor { get; }
        public RpkEditorBase NewEditor { get; }
    }
}