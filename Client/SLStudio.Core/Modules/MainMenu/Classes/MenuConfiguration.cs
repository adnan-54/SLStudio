namespace SLStudio;

public abstract class MenuConfiguration : IMenuConfiguration
{
    protected MenuConfiguration()
    {
    }

    public abstract void BuildMenu(IMenuBuilder builder);
}
