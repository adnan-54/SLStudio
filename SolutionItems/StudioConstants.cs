using System;
using System.Diagnostics;
using System.IO;

internal class StudioConstants
{
    public static readonly string ProductName = "SLStudio";

    public static readonly string LocalKey = $"{ProductName}[{Process.GetCurrentProcess().SessionId}]";
    public static readonly string GlobalKey = $"Global\\{LocalKey}";

    public static readonly string LocalDocumentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ProductName);
    public static readonly string LocalAppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ProductName);
    public static readonly string LocalTempDirectory = Path.Combine(Path.GetTempPath(), ProductName);

    public static readonly string StudioBackupDirectory = Path.Combine(LocalAppDataDirectory, "Backups");
    public static readonly string IconsDirectory = Path.Combine(LocalAppDataDirectory, "Icons");
    public static readonly string LogsDirectory = Path.Combine(LocalAppDataDirectory, "Logs");
    public static readonly string LogsFile = Path.Combine(LogsDirectory, "logs.db");
    public static readonly string InternalLogsFile = Path.Combine(LogsDirectory, "logs.txt");
    public static string NewTempDirectory => GetNewTempDirectory();

    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);
    public static readonly TimeSpan AutoSaveTimeout = TimeSpan.FromSeconds(30);

    private static string GetNewTempDirectory()
    {
        var directory = Path.Combine(LocalTempDirectory, Guid.NewGuid().ToString());
        while (Directory.Exists(directory))
            directory = Path.Combine(LocalTempDirectory, Guid.NewGuid().ToString());

        return directory;
    }
}