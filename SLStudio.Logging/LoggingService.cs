using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    internal class LoggingService
    {
        private bool isInitialized;
        private readonly string logsPath;
        private readonly string dbFilePath;
        private readonly string txtFilePath;
        private readonly SQLiteConnection dbConnection;

        public LoggingService()
        {
            isInitialized = false;
            logsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SLStudio", "Logs");
            dbFilePath = Path.Combine(logsPath, "logs.db");
            txtFilePath = Path.Combine(logsPath, "logs.txt");
            dbConnection = new SQLiteConnection($"Data Source={dbFilePath}; Version=3; datetimeformat=CurrentCulture");
        }

        internal async Task<Log> CreateLog(string title, string message, string sender, LogLevel level)
        {
            if (!isInitialized)
                await Initialize();

            var log = new Log(title, message, sender, $"{level}", DateTime.Now);
            try
            {
                await InsertIntoDb(log);
                return log;
            }
            catch (Exception exception)
            {
                await LogToSimpleFile(exception, log);
                return null;
            }
        }

        private async Task Initialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;
            try
            {
                CreateDatabase();
                await dbConnection.OpenAsync();
                await EnsureTableExists();
            }
            catch (Exception ex)
            {
                await LogToSimpleFile(ex);
            }
        }

        private void CreateDatabase()
        {
            if (!Directory.Exists(logsPath))
                Directory.CreateDirectory(logsPath);
            if (!File.Exists(dbFilePath))
                SQLiteConnection.CreateFile(dbFilePath);
        }

        private async Task EnsureTableExists()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS LOGS(ID INTEGER PRIMARY KEY, Sender VARCHAR(32), Level VARCHAR(16), Title VARCHAR(128), Message VARCHAR(8192), Date DATETIME);";
            using SQLiteCommand command = new SQLiteCommand(commandString, dbConnection);
            await command.ExecuteNonQueryAsync();
        }

        private async Task InsertIntoDb(Log log)
        {
            var commandString = "INSERT INTO LOGS(Sender, Level, Title, Message, Date) VALUES (@sender, @level, @title, @message, @date);";
            using SQLiteCommand command = new SQLiteCommand(commandString, dbConnection);
            command.Parameters.AddWithValue("@sender", log.Sender);
            command.Parameters.AddWithValue("@level", log.Level);
            command.Parameters.AddWithValue("@title", log.Title);
            command.Parameters.AddWithValue("@message", log.Message);
            command.Parameters.AddWithValue("@date", log.Date);
            await command.ExecuteNonQueryAsync();
        }

        private async Task LogToSimpleFile(Exception exception, Log log = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("---------------------------");
                builder.AppendLine($"Date: {DateTime.Now}");
                builder.AppendLine();
                if (log != null)
                    builder.AppendLine($"Log: {log}{Environment.NewLine}");
                builder.AppendLine($"Message: {exception.Message}");
                builder.AppendLine();
                builder.AppendLine($"Exception: {exception}");
                await File.AppendAllTextAsync(txtFilePath, builder.ToString());
            }
            catch { }
        }
    }
}