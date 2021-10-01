using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

internal class StudioConstants
{
    public const string Namespace = "https://slstudio.app/client/2021/xaml/";
    public const string ProductName = "SLStudio";
    public const string ServiceName = "SLStudio Service";
    public const string UpdaterName = "SLStudio Updateder";
    public const string WebSiteName = "SLStudio Web";
    public const string ApiSiteName = "SLStudio Api";
    public const string StudioExe = "SLStudio.exe";

    public static readonly string Copyright = $"Copyright © {DateTime.Now.Year} adnan54. All rights reserved.";
    public static readonly string LocalKey = $"{ProductName}[{Process.GetCurrentProcess().SessionId}]";
    public static readonly string GlobalKey = $"Global\\{LocalKey}";

    public static readonly string DocumentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ProductName);
    public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ProductName);
    public static readonly string TempDirectory = Path.Combine(Path.GetTempPath(), ProductName);
    public static readonly string BackupDirectory = Path.Combine(AppDataDirectory, "Backups");
    public static readonly string IconsDirectory = Path.Combine(AppDataDirectory, "Icons");
    public static readonly string AppStatesDirectory = Path.Combine(AppDataDirectory, "States");
    public static readonly string LogsDirectory = Path.Combine(AppDataDirectory, "Logs");
    public static readonly string DatabaseDirectory = Path.Combine(AppDataDirectory, "Database");

    public static readonly string TempFileExtension = ".tmp";

    public static readonly string DefaultStateFile = Path.Combine(AppStatesDirectory, "defaultState.bin");
    public static readonly string CurrentStateFile = Path.Combine(AppStatesDirectory, "currentState.bin");
    public static readonly string StudioLogFile = Path.Combine(LogsDirectory, "logs.db");
    public static readonly string InternalLogFile = Path.Combine(LogsDirectory, "logs.txt");
    public static readonly string DatabaseFile = Path.Combine(DatabaseDirectory, "SLStudio.db");

    public static readonly string[] AvailableLanguages = new[] { "en-US", "pt-BR" };
    public static readonly CultureInfo[] AvailableCultures = AvailableLanguages.Select(lang => new CultureInfo(lang)).ToArray();
    public static readonly CultureInfo DefaultCulture = CultureInfo.InstalledUICulture;

    public static readonly string WebUrl = GetWebUrl();
    public static readonly string ApiRoute = $"{WebUrl}/api";
    public static readonly string DiscordUrl = "https://discord.gg/gw8S6xT8qS";
    public static readonly string ClientRepo = "https://github.com/adnan-54/SLStudio";
    public static readonly string WebRepo = "https://github.com/adnan-54/SLStudio-Web";

    public static readonly Version ProductVersion = Assembly.GetEntryAssembly().GetName().Version;
    public static readonly Guid ProductId = Guid.Parse("{53EA78AB-6930-4ED7-8349-CCD3478F2F99}");
    public static readonly Guid UpgradeCode = Guid.Parse("{178FFCF5-6C57-4019-B2E0-DB79A49486D4}");
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(1);
    public static readonly TimeSpan AutoSaveTimeout = TimeSpan.FromSeconds(30);

    private static string GetWebUrl()
    {
#if DEBUG
        return @"http://localhost:3000";
#else
        return @"https://slstudio.app";
#endif
    }
}