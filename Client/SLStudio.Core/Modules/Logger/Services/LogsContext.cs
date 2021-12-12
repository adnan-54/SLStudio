using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SLStudio.Logger;

internal class LogsContext : DbContext
{

    private readonly ILogger internalLogger;

    public LogsContext(ILogger internalLogger)
    {
        this.internalLogger = internalLogger;

        Database.EnsureCreated();
    }

    public DbSet<Log> Logs => Set<Log>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            var connection = new SqliteConnection($"Data Source={LogManager.LogsDatabaseFile};");
            connection.Open();
            optionsBuilder.UseSqlite(connection);
        }
        catch (Exception ex)
        {
            internalLogger.Exception(ex);
        }
    }
}