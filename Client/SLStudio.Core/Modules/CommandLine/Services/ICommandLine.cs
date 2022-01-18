namespace SLStudio;

public interface ICommandLine
{
    IEnumerable<string> Args { get; }

    TModel GetFrom<TModel>()
        where TModel : class;
}
