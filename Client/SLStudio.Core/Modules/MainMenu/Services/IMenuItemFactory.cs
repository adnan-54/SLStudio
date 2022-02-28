namespace SLStudio;

public interface IMenuItemFactory
{
    IMenuItem Item(string path, int index, string title, string? toolTip, object icon);

    IMenuButton Button<THandler, TCommand>(string path, int index, string title, string toolTip, object icon)
        where THandler : class, IMenuButtonHandler
        where TCommand : class, IStudioCommand;

    IMenuToggle Toggle<THandler, TCommand>(string path, int index, string title, string toolTip, object icon)
        where THandler : class, IMenuToggleHandler
        where TCommand : class, IStudioToggleCommand;

    IMenuSeparator Separator(string path, int index);
}
