using DevExpress.Mvvm;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkEditorBase : ViewModelBase
    {
        public string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public string IconSource
        {
            get => GetProperty(() => IconSource);
            set => SetProperty(() => IconSource, value);
        }
    }
}