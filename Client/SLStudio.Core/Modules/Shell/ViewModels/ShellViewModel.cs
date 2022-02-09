namespace SLStudio;

internal class ShellViewModel : WindowViewModel, IShell
{
    public ShellViewModel(IMainMenu mainMenu)
    {
        MainMenu = mainMenu;

        Title = "SLStudio";
    }

    public IMainMenu MainMenu { get; }
}
