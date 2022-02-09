using System.Windows.Controls;

namespace SLStudio;

public class ViewContent : ContentControl, IView
{
    private IViewModel? viewModel;

    protected ViewContent()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        DataContextChanged += OnDataContextChanged;
    }

    void IView.PerformAction(Action<ContentControl> action)
    {
        Dispatcher.Invoke(() => action?.Invoke(this));
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        DetachViewModel();
        if (e.NewValue is IViewModel viewModel)
            AttachViewModel(viewModel);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        viewModel?.OnLoaded();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        viewModel?.OnUnloaded();
    }

    private void DetachViewModel()
    {
        if (IsLoaded)
            viewModel?.OnUnloaded();
        viewModel = null;
    }

    private void AttachViewModel(IViewModel newViewModel)
    {
        viewModel = newViewModel;
        if (IsLoaded)
            viewModel?.OnLoaded();
    }
}
