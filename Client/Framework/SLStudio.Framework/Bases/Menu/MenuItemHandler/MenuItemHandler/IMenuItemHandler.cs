namespace SLStudio
{
    public interface IMenuItemHandler<TMenuItem>
        where TMenuItem : class, IMenuItem
    {
        TMenuItem Menu { get; }

        void SetMenu(TMenuItem menu);
    }
}
