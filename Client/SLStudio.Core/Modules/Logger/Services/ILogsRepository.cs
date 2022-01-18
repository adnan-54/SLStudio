namespace SLStudio.Logger;

public interface ILogsRepository : IDisposable
{
    Task AddLog(Log log);

    Task AddLogs(IEnumerable<Log> logs);

    Task<Log?> GetLog(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Log>> GetLogs(CancellationToken cancellationToken = default);

    Task DeleteLogs();

    internal void DumpQueue();
}