namespace SLStudio;

public interface IWindowViewModel : IViewModel
{
    event EventHandler? Activated;

    event EventHandler? Deactivated;

    event EventHandler<CancelEventArgs>? Closing;

    event EventHandler? Closed;

    string? Title { get; }

    bool IsActivated { get; }

    internal void OnActivated();

    internal void OnDeactivated();

    internal void OnClosing(CancelEventArgs args);

    internal void OnClosed();
}
