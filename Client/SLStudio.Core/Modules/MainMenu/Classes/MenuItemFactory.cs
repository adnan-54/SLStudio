namespace SLStudio;

internal class MenuItemFactory : IMenuItemFactory
{
    private readonly IObjectFactory objectFactory;
    private readonly ICommandStorage commandStorage;

    public MenuItemFactory(IObjectFactory objectFactory, ICommandStorage commandStorage)
    {
        this.objectFactory = objectFactory;
        this.commandStorage = commandStorage;
    }

    public IMenuItem Item(string path, int index, string title, string? toolTip, object? icon)
    {
        return new MenuItem(index, path, title, toolTip, icon);
    }

    IMenuButton IMenuItemFactory.Button<THandler>(string path, int index, string title, string toolTip, object icon, string commandKey)
    {
        var handler = objectFactory.GetObject<THandler>();
        var button = new MenuButton(index, path, title, toolTip, icon, handler);
        var command = commandStorage.GetFromKey(commandKey);

        handler.AttachMenu(button);
        handler.AttachCommand(command);

        return button;
    }

    IMenuToggle IMenuItemFactory.Toggle<THandler>(string path, int index, string title, string toolTip, object icon, string commandKey)
    {
        var handler = objectFactory.GetObject<THandler>();
        var toggle = new MenuToggle(index, path, title, toolTip, icon, handler);
        var command = commandStorage.GetFromKey(commandKey);

        handler.AttachMenu(toggle);
        handler.AttachCommand(command);

        return toggle;
    }

    public IMenuSeparator Separator(string path, int index)
    {
        return new MenuSeparator(index, path);
    }
}
