namespace SLStudio;

public abstract class ViewModelBase : DevExpress.Mvvm.ViewModelBase, IViewModel
{
    public event EventHandler? Loaded;
    public event EventHandler? Unloaded;

    public bool IsLoaded
    {
        get => GetValue<bool>();
        private set => SetValue(value);
    }

    void IViewModel.OnLoaded()
    {
        Loaded?.Invoke(this, EventArgs.Empty);
        IsLoaded = true;
    }

    void IViewModel.OnUnloaded()
    {
        Unloaded?.Invoke(this, EventArgs.Empty);
        IsLoaded = false;
    }
}
