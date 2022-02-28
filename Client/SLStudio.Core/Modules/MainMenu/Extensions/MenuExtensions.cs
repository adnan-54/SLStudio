using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

public static class MenuExtensions
{
    public static IConfigurationContext AddMenuConfiguration<TConfiguration>(this IConfigurationContext context)
        where TConfiguration : class, IMenuConfiguration
    {
        var menuService = context.GetService<IMenuService>()!;
        context.AddSingleton<TConfiguration>();
        menuService.AddConfiguration<TConfiguration>();
        return context;
    }
}
