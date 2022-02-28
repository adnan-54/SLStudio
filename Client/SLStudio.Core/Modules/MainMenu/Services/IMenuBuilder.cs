using System.Resources;

namespace SLStudio;

public interface IMenuBuilder
{
    void UseResourceContext(ResourceManager resourceManager);

    void Item(string path, int? index = null, string? title = null, string? toolTip = null, object? icon = null);

    void Button<TCommand>(string path, int? index = null, string? title = null, string? toolTip = null, object? icon = null)
        where TCommand : class, IStudioCommand;

    void Button<THandler, TCommand>(string path, int? index = null, string? title = null, string? toolTip = null, object? icon = null)
        where THandler : class, IMenuButtonHandler
        where TCommand : class, IStudioCommand;

    void Toggle<TCommand>(string path, string commandKey, int? index = null, string? title = null, string? toolTip = null, object? icon = null)
        where TCommand : class, IStudioCommand;

    void Toggle<THandler, TCommand>(string path, string commandKey, int? index = null, string? title = null, string? toolTip = null, object? icon = null)
        where THandler : class, IMenuToggleHandler
        where TCommand : class, IStudioToggleCommand;

    void Separator(string path, int? index = null);
}