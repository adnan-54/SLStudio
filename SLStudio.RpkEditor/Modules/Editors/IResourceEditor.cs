namespace SLStudio.RpkEditor.Editors
{
    internal interface IResourceEditor
    {
        bool IsValid { get; }
        void ApplyChanges();
    }
}