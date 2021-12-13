using System.ComponentModel;

namespace SLStudio.Mvvm;

public interface IViewModel : INotifyPropertyChanged
{
    
}

public interface IComponentViewModel : IViewModel
{
    event EventHandler Loaded;

    event EventHandler Unloaded;
}

public interface IWindowViewModel : IComponentViewModel
{

}

public interface IDialogViewModel<TResult> : IWindowViewModel
    where TResult : class
{
    event EventHandler<ResultChangedEventArgs<TResult>> ResultChanged;

    TResult? Result { get; }

    bool HasResult { get; }

    TResult? GetResultOrDefault(TResult? defaultResult = default);
}
