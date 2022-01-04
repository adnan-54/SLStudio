namespace SLStudio;

public interface IDispatcher
{
	void Invoke(Action callback);

	void Invoke(Action callback, DispatcherPriority priority);

	void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken);

	DispatcherOperation InvokeAsync(Action callback);

	DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority);

	DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken);

	DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback);

	DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority);

	DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken);

	TResult Invoke<TResult>(Func<TResult> callback);

	TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority);

	TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken);
}