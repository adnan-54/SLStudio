namespace SLStudio;

public abstract class DialogViewModel<TResult> : WindowViewModel, IDialogViewModel<TResult>
    where TResult : class
{
    public event EventHandler<ResultChangedEventArgs<TResult?>>? ResultChanged;

    public TResult? Result
    {
        get => GetValue<TResult?>();
        set => SetResult(Result, value);
    }

    public bool HasResult => Result is not null;

    public TResult? GetResultOrDefault(TResult? defaultResult)
    {
        if (HasResult)
            return Result;
        return defaultResult;
    }

    public bool TryGetResult(out TResult? result)
    {
        result = Result;
        return HasResult;
    }

    private void SetResult(TResult? oldResult, TResult? newResult)
    {
        if (SetValue(newResult))
        {
            ResultChanged?.Invoke(this, new(oldResult, newResult));
            RaisePropertyChanged(nameof(HasResult));
        }
    }
}