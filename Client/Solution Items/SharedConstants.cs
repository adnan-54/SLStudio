using System.Diagnostics;
using System.Reflection;

namespace SLStudio;

public static class SharedConstants
{
    public const string Namespace = "https://slstudio.app/client/2021/xaml/";
    public static readonly string ApplicationName = "SLStudio";
    public static readonly Version? ApplicationVersion = Assembly.GetEntryAssembly()?.GetName()!.Version;

    public static readonly string LocalKey = $"{ApplicationName}[{Process.GetCurrentProcess().SessionId}]";
    public static readonly string GlobalKey = $"Global\\{LocalKey}";

    public static readonly string DocumentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ApplicationName);
    public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);
    public static readonly string TempDirectory = Path.Combine(Path.GetTempPath(), ApplicationName);

    public static readonly string WebUrl = GetWebUrl();
    public static readonly string ApiRouteUrl = $"{WebUrl}/api";
    public static readonly string DiscordUrl = "https://discord.gg/gw8S6xT8qS";
    public static readonly string RepositoryUrl = "https://github.com/adnan-54/SLStudio";

    private static string GetWebUrl()
    {
#if DEBUG
        return @"http://localhost:3000";
#else
        return @"https://slstudio.app";
#endif
    }
}
