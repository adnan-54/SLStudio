namespace SLStudio;

public interface IModule
{
    string Name { get; }

    string Description { get; }

    string License { get; }

    string Author { get; }

    Version Version { get; }

    int Priority { get; }

    void Configure(IConfigurationContext context);
}
