namespace SLStudio.Mvvm;

public class ResultChangedEventArgs<TResult> : EventArgs
{
    public ResultChangedEventArgs(TResult? oldResult, TResult? newResult)
    {
        OldResult = oldResult;
        NewResult = newResult;
    }

    public TResult? OldResult { get; }

    public TResult? NewResult { get; }
}
