using System.ComponentModel;

namespace SLStudio.RpkEditor.Editors
{
    internal interface IDefinitionEditor
    {
        bool IsValid { get; }

        bool Validate();

        void ApplyChanges();
    }
}