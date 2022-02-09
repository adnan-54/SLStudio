using System.Windows.Controls;

namespace SLStudio;

public class ModalWindow : Window, IWindowView
{
    private IWindowViewModel? viewModel;

    protected ModalWindow()
    {
        DataContextChanged += OnDataContextChanged;
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Activated += OnActivated;
        Deactivated += OnDeactivated;
        Closing += OnClosing;
        Closed += OnClosed;
    }

    public WindowState State => WindowState;

    void IView.PerformAction(Action<ContentControl> action)
    {
        Dispatcher.Invoke(() => action?.Invoke(this));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        ApplyDefaultStyle();
    }

    private void ApplyDefaultStyle()
    {
        var style = Application.Current.TryFindResource("StudioWindowStyle") as Style;
        Style = style;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        DetachViewModel();
        if (e.NewValue is IWindowViewModel newViewModel)
            AttachViewModel(newViewModel);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        viewModel?.OnLoaded();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        viewModel?.OnUnloaded();
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        viewModel?.OnActivated();
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        viewModel?.OnDeactivated();
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        viewModel?.OnClosing(e);
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        viewModel?.OnClosed();
    }

    private void DetachViewModel()
    {
        if (IsLoaded)
            viewModel?.OnUnloaded();
        if (IsActive)
            viewModel?.OnDeactivated();

        viewModel = null;
    }

    private void AttachViewModel(IWindowViewModel newViewModel)
    {
        viewModel = newViewModel;

        if (IsLoaded)
            viewModel?.OnLoaded();
        if (IsActive)
            viewModel?.OnActivated();
    }
}
