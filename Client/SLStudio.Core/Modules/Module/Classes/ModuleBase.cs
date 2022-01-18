namespace SLStudio;

public abstract class ModuleBase : IModule
{
    private string? toString;
    private bool isConfigured;

    public abstract string Name { get; }

    public abstract string Description { get; }

    public abstract string License { get; }

    public abstract string Author { get; }

    public abstract Version Version { get; }

    public abstract int Priority { get; }

    public void Configure(IConfigurationContext context)
    {
        if (isConfigured)
            return;

        OnConfigure(context);

        isConfigured = true;
    }

    protected abstract void OnConfigure(IConfigurationContext context);

    public override string ToString()
    {
        if (string.IsNullOrEmpty(toString))
        {
            var sb = new StringBuilder();

            sb.Append('{');
            sb.Append($"\"{nameof(Name)}\": \"{Name}\"; ");
            sb.Append($"\"{nameof(Description)}\": \"{Description}\"; ");
            sb.Append($"\"{nameof(License)}\": \"{License}\"; ");
            sb.Append($"\"{nameof(Author)}\": \"{Author}\"; ");
            sb.Append($"\"{nameof(Version)}\": \"{Version}\"; ");
            sb.Append($"\"{nameof(Priority)}\": \"{Priority}\"; ");
            sb.Append('}');

            toString = sb.ToString();
        }

        return toString;
    }
}
