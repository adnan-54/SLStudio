using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

public static class CommandLineExtensions
{

    public static IConfigurationContext AddCommandLine<TModel>(this IConfigurationContext context) where TModel : class
    {
        var commandLine = context.GetService<ICommandLine>()!;
        context.AddSingleton(() => commandLine.GetFrom<TModel>());
        return context;
    }
}
