namespace SLStudio.Core.Menus
{
    internal class MenuSeparatorItem : MenuItem, IMenuSeparatorItem
    {
        public MenuSeparatorItem(string path, int index) : base(path, index)
        {
        }
    }

    public interface IMenuSeparatorItem : IMenuItem
    {
    }
}