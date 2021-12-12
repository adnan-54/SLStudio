namespace SLStudio;

public abstract class ModuleBase : IModule
{
    private bool isConfigured;

    public abstract string Name { get; }

    public abstract string Description { get; }

    public abstract string License { get; }

    public abstract string Author { get; }

    public abstract Version Version { get; }

    public abstract int Priority { get; }

    public void Configure(IApplicationContext context)
    {
        if (isConfigured)
            return;

        OnConfigure(context);

        isConfigured = true;
    }

    protected abstract void OnConfigure(IApplicationContext context);
}
