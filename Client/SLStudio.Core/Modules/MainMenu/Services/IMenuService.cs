namespace SLStudio;

internal interface IMenuService
{
    IEnumerable<IMenuItem> MenuItems { get; }

    void AddConfiguration<TConfiguration>()
        where TConfiguration : class, IMenuConfiguration;
}