namespace SLStudio;

public interface IMenuItem
{
    IMenuItem? Parent { get; }

    IEnumerable<IMenuItem> Children { get; }

    int Index { get; }

    string Path { get; }

    string Title { get; set; }

    string? ToolTip { get; set; }

    object? Icon { get; set; }

    bool IsRootItem { get; }

    bool IsVisible { get; set; }

    bool IsEnabled { get; set; }

    internal void SetParent(IMenuItem parent);

    internal void AddChild(IMenuItem child);
}
