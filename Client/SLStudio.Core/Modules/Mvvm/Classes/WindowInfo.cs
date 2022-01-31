namespace SLStudio;

internal class WindowInfo : IWindowInfo
{
    public WindowInfo(IWindowView window, IWindowViewModel viewModel, bool isModal)
    {
        Window = window;
        ViewModel = viewModel;
        IsModal = isModal;
    }

    public IWindowView Window { get; }

    public IWindowViewModel ViewModel { get; }

    public bool IsModal { get; }

    public bool IsDialog => !IsModal;
}
