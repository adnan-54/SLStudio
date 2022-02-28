namespace SLStudio;

public interface IMenuToggle : IMenuItem
{
    IMenuToggleHandler Handler { get; }

    bool IsChecked { get; set; }
}
