using System.ComponentModel;

namespace SLStudio.Mvvm;

public interface IViewModel : INotifyPropertyChanged
{
    event EventHandler Loaded;

    event EventHandler Unloaded;
}

public interface IWindowViewModel : IViewModel
{
    event EventHandler<CancelEventArgs> Closing;

    event EventHandler Closed;

    string Title { get; set; }
}

public interface IDialogViewModel<TResult> : IWindowViewModel
    where TResult : class
{
    event EventHandler<ResultChangedEventArgs<TResult>> ResultChanged;

    TResult? Result { get; }

    bool HasResult { get; }

    TResult? GetResultOrDefault(TResult? defaultResult = default);
}