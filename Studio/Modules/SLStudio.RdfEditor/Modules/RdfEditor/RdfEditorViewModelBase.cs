using DevExpress.Mvvm;

namespace SLStudio.RdfEditor.Modules.RdfEditor.ViewModels
{
    internal class RdfEditorViewModelBase : ViewModelBase
    {
        public string DisplayName { get; init; }
        public string IconSource { get; init; }
    }
}