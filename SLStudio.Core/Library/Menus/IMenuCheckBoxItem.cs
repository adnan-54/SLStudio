namespace SLStudio.Core.Menus
{
    internal class MenuToggleItem : MenuItem, IMenuToggleItem
    {
        public bool IsChecked
        {
            get => GetProperty(() => IsChecked);
            set
            {
                if (IsChecked == value)
                    return;
                SetProperty(() => IsChecked, value);
            }
        }
    }

    public interface IMenuToggleItem : IMenuItem
    {
        bool IsChecked { get; set; }
    }
}