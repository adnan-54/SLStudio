namespace SLStudio;

public abstract class WindowViewModel : ViewModelBase, IWindowViewModel
{
    public event EventHandler? Activated;
    public event EventHandler? Deactivated;
    public event EventHandler<CancelEventArgs>? Closing;
    public event EventHandler? Closed;

    public string? Title
    {
        get => GetValue<string>();
        protected set => SetValue(value);
    }

    public bool IsActivated
    {
        get => GetValue<bool>();
        private set => SetValue(value);
    }

    void IWindowViewModel.OnActivated()
    {
        Activated?.Invoke(this, EventArgs.Empty);
        IsActivated = true;
    }

    void IWindowViewModel.OnDeactivated()
    {
        Deactivated?.Invoke(this, EventArgs.Empty);
        IsActivated = false;
    }

    void IWindowViewModel.OnClosing(CancelEventArgs args)
    {
        Closing?.Invoke(this, args);
    }

    void IWindowViewModel.OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}
