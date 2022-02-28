namespace SLStudio;

internal class MenuToggle : MenuItem, IMenuToggle
{
    public MenuToggle(int index, string path, string title, string toolTip, object icon, IMenuToggleHandler handler)
        : base(index, path, title, toolTip, icon)
    {
        Handler = handler;
    }

    public IMenuToggleHandler Handler { get; }

    public bool IsChecked
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }
}
