namespace SLStudio.Studio.Commands;

internal class CommandsModule : ISubModule
{
    public void OnConfigure(IConfigurationContext context)
    {
        context.AddCommand<CreateNewFileCommand>();
    }
}
