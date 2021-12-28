namespace SLStudio;

public interface IDispatcher
{
    public void Invoke(Action callback)
    {
    }

    public void Invoke(Action callback, DispatcherPriority priority)
    {
    }

    public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
    }

    public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
    {
    }

    public object Invoke(Delegate method, params object[] args)
    {
        throw null;
    }

    public object Invoke(Delegate method, TimeSpan timeout, params object[] args)
    {
        throw null;
    }

    public object Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args)
    {
        throw null;
    }

    public object Invoke(Delegate method, DispatcherPriority priority, params object[] args)
    {
        throw null;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object Invoke(DispatcherPriority priority, Delegate method)
    {
        throw null;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object Invoke(DispatcherPriority priority, Delegate method, object arg)
    {
        throw null;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object Invoke(DispatcherPriority priority, Delegate method, object arg, params object[] args)
    {
        throw null;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method)
    {
        throw null;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object arg)
    {
        throw null;
    }

    public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object arg, params object[] args)
    {
        throw null;
    }

    public DispatcherOperation InvokeAsync(Action callback)
    {
        throw null;
    }

    public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority)
    {
        throw null;
    }

    public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        throw null;
    }

    public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback)
    {
        throw null;
    }

    public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority)
    {
        throw null;
    }

    public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        throw null;
    }

    public void InvokeShutdown()
    {
    }

    public TResult Invoke<TResult>(Func<TResult> callback)
    {
        throw null;
    }

    public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority)
    {
        throw null;
    }

    public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        throw null;
    }

    public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
    {
        throw null;
    }



}