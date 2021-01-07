using System;
using System.IO;

internal class SLStudioConstants
{
    public const string ProductName = "SLStudio";

    public static readonly string LocalDocumentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ProductName);
    public static readonly string LocalAppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ProductName);

    public static readonly string StudioBackupDirectory = Path.Combine(LocalAppDataDirectory, "Backups");
    public static readonly string IconsDirectory = Path.Combine(LocalAppDataDirectory, "Icons");
    public static readonly string LogsDirectory = Path.Combine(LocalAppDataDirectory, "Logs");
    public static readonly string LogFilePath = Path.Combine(LogsDirectory, "logs.db");
    public static readonly string SimpleLogsFilePath = Path.Combine(LogsDirectory, "logs.txt");

    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);
    public static readonly TimeSpan AutoSaveTimeout = TimeSpan.FromSeconds(30);
}