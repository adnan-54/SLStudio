using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Logging
{
    internal class DefaultLogRespository : ILogRespository
    {
        private readonly LogContext context;
        private readonly int maxResults;

        public DefaultLogRespository(IConfigurationService configurationService)
        {
            context = new LogContext();

            maxResults = configurationService.MaxRetrieveResults;
            if (maxResults <= 0)
                maxResults = int.MaxValue;
        }

        public IEnumerable<Log> GetAll()
        {
            return context.Logs.Take(maxResults).OrderByDescending(log => log.Id).ToList();
        }

        public void Add(Log log)
        {
            context.Logs.Add(log);
            context.SaveChanges();
        }

        public void DeleteAll()
        {
            context.Database.ExecuteSqlRaw("DELETE FROM Logs");
        }

        private class LogContext : DbContext
        {
            public DbSet<Log> Logs { get; set; }

            public LogContext()
            {
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                var connection = new SqliteConnection($"Data Source={SharedConstants.StudioLogFile};Password=J_C#,qU^Ew4f[t;");
                connection.Open();
                optionsBuilder.UseSqlite(connection);
            }
        }
    }

    internal interface ILogRespository
    {
        IEnumerable<Log> GetAll();

        void Add(Log log);

        void DeleteAll();
    }
}