namespace SLStudio;

internal class MainMenuViewModel : ViewModelBase, IMainMenu
{
    private readonly IMenuService menuService;

    public MainMenuViewModel(IMenuService menuService)
    {
        this.menuService = menuService;
    }

    public IEnumerable<IMenuItem> Menus => menuService.MenuItems;
}
