using Microsoft.EntityFrameworkCore;

namespace SLStudio.Logger;

internal partial class LogsRepository : ILogsRepository
{
    private readonly ILogManager logManager;
    private readonly ILogger internalLogger;
    private readonly LogsContext logsContext;
    private readonly List<Log> logsCache;
    private bool isDisposed;

    public LogsRepository(ILogManager logManager, ILogger internalLogger)
    {
        this.logManager = logManager;
        this.internalLogger = internalLogger;
        logsContext = new(internalLogger);
        logsCache = new();

        logManager.Initialized += OnLogManagerInitialized;
    }

    public async Task AddLog(Log log)
    {
        try
        {
            if (!logManager.IsInitialized)
            {
                logsCache.Add(log);
                return;
            }

            logsContext.Logs.Add(log);
            await logsContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }
    }

    public async Task AddLogs(IEnumerable<Log> logs)
    {
        try
        {
            if (!logManager.IsInitialized)
            {
                logsCache.AddRange(logs);
                return;
            }

            logsContext.AddRange(logs);
            await logsContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }
    }

    public Task<Log?> GetLog(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            if (logManager.IsInitialized)
                return logsContext.Logs.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }

        return Task.FromResult<Log?>(null);
    }

    public async Task<IEnumerable<Log>> GetLogs(CancellationToken cancellationToken)
    {
        try
        {
            return await logsContext.Logs.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }

        return Enumerable.Empty<Log>();
    }

    public async Task DeleteLogs()
    {
        try
        {
            await logsContext.Database.CloseConnectionAsync();
            await logsContext.Database.EnsureDeletedAsync();
            await logsContext.Database.EnsureCreatedAsync();
            await logsContext.Database.OpenConnectionAsync();

        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }
    }

    void ILogsRepository.DumpQueue()
    {
        try
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(LogManager.LineSeparator);
            stringBuilder.AppendLine($"Emergency dump requested at {DateTime.Now}");
            stringBuilder.AppendLine($"Dumping all {logsCache.Count} log(s) in cache:");
            stringBuilder.AppendLine();

            foreach (var log in logsCache)
            {
                stringBuilder.Append(log.ToString());
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine(LogManager.LineSeparator);

            File.AppendAllText(LogManager.LogsOutputFile, stringBuilder.ToString());

        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
                logsContext.Dispose();
            isDisposed = true;
        }
    }

    private async void OnLogManagerInitialized(object? sender, EventArgs e)
    {
        logManager.Initialized -= OnLogManagerInitialized;
        await AddLogs(logsCache);
    }
}
