namespace SLStudio;

internal class SLStudioDispatcher : IDispatcher
{
    private readonly Dispatcher dispatcher;

    public SLStudioDispatcher(Dispatcher dispatcher)
    {
        this.dispatcher = dispatcher;
    }

    public void Invoke(Action callback)
    {
        dispatcher.Invoke(callback);
    }

    public void Invoke(Action callback, DispatcherPriority priority)
    {
        dispatcher.Invoke(callback, priority);
    }

    public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        dispatcher.Invoke(callback, priority, cancellationToken);
    }

    public DispatcherOperation InvokeAsync(Action callback)
    {
        return dispatcher.InvokeAsync(callback);
    }

    public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority)
    {
        return dispatcher.InvokeAsync(callback, priority);
    }

    public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        return dispatcher.InvokeAsync(callback, priority, cancellationToken);
    }

    public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback)
    {
        return dispatcher.InvokeAsync(callback);
    }

    public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority)
    {
        return dispatcher.InvokeAsync(callback, priority);
    }

    public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        return dispatcher.InvokeAsync(callback, priority, cancellationToken);
    }

    public TResult Invoke<TResult>(Func<TResult> callback)
    {
        return dispatcher.Invoke(callback);
    }

    public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority)
    {
        return dispatcher.Invoke(callback, priority);
    }

    public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        return dispatcher.Invoke(callback, priority, cancellationToken);
    }
}