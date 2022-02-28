namespace SLStudio;

public interface IMenuButton : IMenuItem
{
    IMenuButtonHandler Handler { get; }
}
