namespace SLStudio;

public interface IViewModel : INotifyPropertyChanged
{
    event EventHandler? Loaded;

    event EventHandler? Unloaded;

    bool IsLoaded { get; }

    internal void OnLoaded();

    internal void OnUnloaded();
}
