namespace SLStudio;

public interface IWindowView : IView
{
    event EventHandler Activated;

    event EventHandler Deactivated;

    event CancelEventHandler Closing;

    event EventHandler Closed;

    string Title { get; }

    bool IsActive { get; }

    WindowState State { get; }

    internal void PerformAction(Action<Window> action);
}
