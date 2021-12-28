using System.Runtime.CompilerServices;

namespace SLStudio.Logger;

public static class LogManagerExtensions
{
    public static ILogger GetLogger<TType>(this ILogManager logManager) where TType : class
    {
        return logManager.GetLogger(nameof(TType));
    }

    public static ILogger GetLogger(this ILogManager logManager, [CallerFilePath] string? name = default)
    {
        if (!string.IsNullOrEmpty(name) && name.Contains(Path.DirectorySeparatorChar) && name.EndsWith(".cs"))
            name = Path.GetFileNameWithoutExtension(name);
        return logManager.GetLogger(name);
    }
}
