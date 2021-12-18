namespace SLStudio.Logger;

public partial class LogManager
{
    internal static readonly string LogsDirectory = Path.Combine(SharedConstants.AppDataDirectory, "Logs");
    internal static readonly string LogsDatabaseFile = Path.Combine(LogsDirectory, "logs.db");
    internal static readonly string LogsOutputFile = Path.Combine(LogsDirectory, "output.txt");
    internal static readonly string InternalLoggerSeparator = new('-', 50);

    public static readonly ILogManager Default = new LogManager();
}