namespace SLStudio;

internal class MenuItem : BindableBase, IMenuItem
{
    private List<IMenuItem> children;

    public MenuItem(int index, string path) : this(index, path, string.Empty, null, null)
    {
    }

    public MenuItem(int index, string path, string title, string? toolTip, object? icon)
    {
        children = new();
        Index = index;
        Path = path;
        Title = title;
        ToolTip = toolTip;
        Icon = icon;
        IsVisible = true;
        IsEnabled = true;
    }

    public IMenuItem? Parent { get; private set; }

    public IEnumerable<IMenuItem> Children => children;

    public int Index { get; }

    public string Path { get; }

    public string Title
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? ToolTip
    {
        get => GetValue<string?>();
        set => SetValue(value);
    }

    public object? Icon
    {
        get => GetValue<object>();
        set => SetValue(value);
    }

    public bool IsRootItem => Parent is null;

    public bool IsVisible
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsEnabled
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    void IMenuItem.SetParent(IMenuItem parent)
    {
        if (parent is not null)
            throw new InvalidOperationException("Parent has been already set");
        Parent = parent;
    }

    void IMenuItem.AddChild(IMenuItem child)
    {
        children.Add(child);
        children = new(children.OrderBy(m => m.Index));
    }
}
