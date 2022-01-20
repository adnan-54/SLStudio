namespace SLStudio;

public interface IWindowManager
{
    void ShowModal<TViewModel>()
        where TViewModel : class, IWindowViewModel;

    void ShowModal<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    void ShowDialog<TViewModel>()
        where TViewModel : class, IWindowViewModel;

    void ShowDialog<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    void Activate<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    void Maximize<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    void Restore<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    void Minimize<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    void Close<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;
}

internal interface IViewModelCache
{

}

internal interface IViewCache
{

}