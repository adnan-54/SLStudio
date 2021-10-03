namespace SLStudio
{
    public interface IMenuToggle : IMenuButton
    {
        bool IsChecked { get; }
    }
}