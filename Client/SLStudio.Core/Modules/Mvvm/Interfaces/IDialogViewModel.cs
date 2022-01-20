namespace SLStudio;

public interface IDialogViewModel<TResult> : IWindowViewModel
    where TResult : class
{
    event EventHandler<ResultChangedEventArgs<TResult?>>? ResultChanged;

    TResult? Result { get; }

    bool HasResult { get; }

    TResult? GetResultOrDefault(TResult? defaultResult = default);

    bool TryGetResult(out TResult? result);
}