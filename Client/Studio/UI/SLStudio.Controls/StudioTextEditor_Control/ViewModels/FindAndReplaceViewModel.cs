using DevExpress.Mvvm;

namespace SLStudio.Core
{
    internal class FindAndReplaceViewModel : BindableBase
    {
        public FindAndReplaceViewModel()
        {
        }

        public FindAndReplaceViewModel(IFindReplaceService findReplace)
        {
            FindReplace = findReplace;
        }

        public bool IsOpen
        {
            get => GetProperty(() => IsOpen);
            set => SetProperty(() => IsOpen, value);
        }

        public IFindReplaceService FindReplace { get; }

        public void Show()
        {
            IsOpen = true;
        }

        public void ShowFind()
        {
            Show();
            FindReplace.IsReplacing = false;
        }

        public void ShowFindReplace()
        {
            Show();
            FindReplace.IsReplacing = true;
        }

        public void Close()
        {
            IsOpen = false;
        }
    }
}