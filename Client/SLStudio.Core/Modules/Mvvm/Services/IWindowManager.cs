namespace SLStudio;

public interface IWindowManager
{
    IEnumerable<IWindowInfo> Windows { get; }

    IWindowInfo? MainWindow { get; }

    IWindowInfo? CurrentWindow { get; }

    TViewModel ShowModal<TViewModel>()
        where TViewModel : class, IWindowViewModel;

    TViewModel ShowModal<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel;

    TViewModel ShowDialog<TViewModel>()
        where TViewModel : class, IWindowViewModel;

    TViewModel ShowDialog<TViewModel>(TViewModel viewModel)
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
