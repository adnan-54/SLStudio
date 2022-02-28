namespace SLStudio;

public interface IMainMenu : IViewModel
{
    IEnumerable<IMenuItem> Menus { get; }
}
