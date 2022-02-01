namespace SLStudio;

public class StudioWindow : Window, IWindowView
{
    private IWindowViewModel? viewModel;

    protected StudioWindow()
    {
        DataContextChanged += OnDataContextChanged;
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Activated += OnActivated;
        Deactivated += OnDeactivated;
        Closing += OnClosing;
        Closed += OnClosed;
    }

    event EventHandler IWindowView.Activated
    {
        add => Activated += value;
        remove => Activated -= value;
    }

    event EventHandler IWindowView.Deactivated
    {
        add => Deactivated += value;
        remove => Deactivated -= value;
    }

    event CancelEventHandler IWindowView.Closing
    {
        add => Closing += value;
        remove => Closing -= value;
    }

    event EventHandler IWindowView.Closed
    {
        add => Closed += value;
        remove => Closed -= value;
    }

    event DependencyPropertyChangedEventHandler IView.DataContextChanged
    {
        add => DataContextChanged += value;
        remove => DataContextChanged -= value;
    }

    event EventHandler IView.Initialized
    {
        add => Initialized += value;
        remove => Initialized -= value;
    }

    event RoutedEventHandler IView.Loaded
    {
        add => Loaded += value;
        remove => Loaded -= value;
    }

    event RoutedEventHandler IView.Unloaded
    {
        add => Unloaded += value;
        remove => Unloaded -= value;
    }

    string IWindowView.Title => Title;

    bool IWindowView.IsActive => IsActive;

    WindowState IWindowView.State => WindowState;

    object IView.DataContext => DataContext;

    bool IView.IsInitialized => IsInitialized;

    bool IView.IsLoaded => IsLoaded;

    bool IView.IsEnabled => IsEnabled;

    bool IView.IsFocused => IsFocused;

    object IView.Tag => Tag;

    Visibility IView.Visibility => Visibility;

    void IWindowView.PerformAction(Action<Window> action)
    {
        if (action is null)
            return;
        Dispatcher.Invoke(() => action.Invoke(this));
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
        if (e.OldValue is IWindowViewModel oldViewModel)
            DetachViewModel(oldViewModel);
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

    private void AttachViewModel(IWindowViewModel newViewModel)
    {
        if (viewModel is not null)
            return;

        viewModel = newViewModel;

        if (IsLoaded)
            viewModel.OnUnloaded();
        if (IsActive)
            viewModel.OnActivated();
    }

    private void DetachViewModel(IWindowViewModel oldViewModel)
    {
        if (viewModel != oldViewModel)
            return;

        if (IsLoaded)
            viewModel.OnUnloaded();
        if (IsActive)
            viewModel.OnDeactivated();

        viewModel = null;
    }
}
