namespace SLStudio;

internal class MenuSeparator : MenuItem, IMenuSeparator
{
    public MenuSeparator(int index, string path)
        : base(index, path)
    {
    }
}