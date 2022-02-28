namespace SLStudio;

internal class MenuButton : MenuItem, IMenuButton
{
    public MenuButton(int index, string path, string title, string toolTip, object icon, IMenuButtonHandler handler)
        : base(index, path, title, toolTip, icon)
    {
        Handler = handler;
    }

    public IMenuButtonHandler Handler { get; }
}
