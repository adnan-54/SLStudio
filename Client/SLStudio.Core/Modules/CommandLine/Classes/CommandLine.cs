using CommandLine;

namespace SLStudio;

internal class CommandLine : ICommandLine
{
    private readonly IApplication application;
    private readonly Parser parser;

    public CommandLine(IApplication application)
    {
        this.application = application;
        parser = Parser.Default;
    }

    public IEnumerable<string> Args => application.Args;

    TModel ICommandLine.GetFrom<TModel>()
    {
        TModel? model = default;
        var result = parser.ParseArguments<TModel>(Args);
        result.WithParsed(m => model = m);

        if (model is not null)
            return model;

        throw new InvalidOperationException($"Could not parse the commmand line from {typeof(TModel)}");
    }
}