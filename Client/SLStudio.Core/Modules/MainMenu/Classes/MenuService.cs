namespace SLStudio;

internal class MenuService : IMenuService
{
    private readonly IObjectFactory objectFactory;
    private readonly IMenuItemFactory menuItemFactory;
    private readonly List<IMenuConfiguration> configurations;
    private readonly List<IMenuItem> menus;

    public MenuService(IObjectFactory objectFactory, IMenuItemFactory menuItemFactory)
    {
        this.objectFactory = objectFactory;
        this.menuItemFactory = menuItemFactory;
        configurations = new();
        menus = new();
    }

    public IEnumerable<IMenuItem> MenuItems => GetMenuItems();

    void IMenuService.AddConfiguration<TConfiguration>()
    {
        var configuration = objectFactory.GetObject<TConfiguration>();
        if (configurations.Contains(configuration))
            throw new InvalidOperationException($"Menu configuration '{configuration}' has been already added");
        configurations.Add(configuration);
    }

    private IEnumerable<IMenuItem> GetMenuItems()
    {
        if (menus.None())
        {
            var builder = new MenuBuilder(menuItemFactory);
            foreach (var configuration in configurations)
                configuration.BuildMenu(builder);
            menus.AddRange(builder.Build());
        }

        return menus;
    }
}
