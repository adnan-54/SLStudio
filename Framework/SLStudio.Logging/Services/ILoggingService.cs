using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    internal class DefaultLoggingService : ILoggingService
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultLoggingService>();

        private readonly object @lock = new object();
        private readonly LogManager logManager;
        private readonly IInternalLogger internalLogger;
        private readonly IConfigurationService configurationService;
        private readonly FileInfo dbFile;
        private readonly SQLiteConnection dbConnection;
        private readonly LogsProcessor logsProcessor;

        public DefaultLoggingService(LogManager logManager, IInternalLogger internalLogger, IConfigurationService configurationService)
        {
            this.logManager = logManager;
            this.internalLogger = internalLogger;
            this.configurationService = configurationService;

            var filePath = StudioConstants.LogsFile;

            CreateDatabase();

            dbFile = new FileInfo(filePath);
            dbConnection = new SQLiteConnection($"Data Source={dbFile.FullName}; Version=3; datetimeformat=CurrentCulture");
            logsProcessor = new LogsProcessor(this);
        }

        public void EnqueueLog(string sender, string title, string message, string level, string callerFile, string callerMember, int callerLine, string stackTrace)
        {
            var log = CreateLog(title, message, sender, level, callerFile, callerMember, callerLine, stackTrace);
            logsProcessor.EnqueueLog(log);
        }

        public async Task<DataTable> GetLogs()
        {
            await EnsureDatabase();

            var dataTable = new DataTable();

            try
            {
                var commandString = "SELECT * FROM LOGS ORDER BY ID DESC";

                var limit = configurationService.MaxRetrieveResults;
                if (limit > 0)
                    commandString = $"{commandString} LIMIT {limit}";

                using var dataAdapter = new SQLiteDataAdapter(commandString, dbConnection);
                await Task.Run(() =>
                {
                    lock (@lock)
                        dataAdapter.Fill(dataTable);
                });
            }
            catch (Exception exception)
            {
                internalLogger.Log(exception);
            }

            return dataTable;
        }

        public long GetSize()
        {
            dbFile.Refresh();
            return dbFile.Length;
        }

        public async Task DeleteAll()
        {
            try
            {
                if (dbConnection.State != ConnectionState.Closed)
                    await dbConnection.CloseAsync();

                var filePath = dbFile.FullName;

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception ex)
            {
                internalLogger.Log(ex);
            }
            finally
            {
                await EnsureDatabase();
                logger.Info("All logs have been successfully deleted", "Clear Logs");
            }
        }

        private static Log CreateLog(string title, string message, string sender, string level, string callerFile, string callerMember, int callerLine, string stackTrace)
        {
            return new Log(title, message, sender, level, DateTime.Now, callerFile, callerMember, callerLine, stackTrace);
        }

        private Log UpdateLog(Log log)
        {
            if (string.IsNullOrEmpty(log.Level))
                log = log with { Level = $"{configurationService.DefaultLogLevel}" };
            return log;
        }

        private bool CanLog(Log log)
        {
            var miminumLogLevel = configurationService.MinimumLogLevel;
            var currentLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), log.Level, true);

            if (currentLogLevel == LogLevel.Debug && configurationService.IgnoreDebugLevel)
                return false;

            return currentLogLevel >= miminumLogLevel;
        }

        private async Task InsertIntoDb(Log log)
        {
            await EnsureDatabase();

            var commandString = "INSERT INTO LOGS(Sender, Level, Title, Message, Date, CallerFile, CallerMember, CallerLine, StackTrace) VALUES (@sender, @level, @title, @message, @date, @file, @member, @line, @stack);";

            using SQLiteCommand command = new SQLiteCommand(commandString, dbConnection);
            command.Parameters.AddWithValue("@sender", log.Sender);
            command.Parameters.AddWithValue("@level", log.Level);
            command.Parameters.AddWithValue("@title", log.Title);
            command.Parameters.AddWithValue("@message", log.Message);
            command.Parameters.AddWithValue("@date", log.Date);
            command.Parameters.AddWithValue("@file", log.CallerFile);
            command.Parameters.AddWithValue("@member", log.CallerMember);
            command.Parameters.AddWithValue("@line", log.CallerLine);
            command.Parameters.AddWithValue("@stack", log.StackTrace);

            await command.ExecuteNonQueryAsync();
        }

        private async Task EnsureDatabase()
        {
            try
            {
                if (dbConnection.State == ConnectionState.Open)
                    return;

                CreateDatabase();
                await dbConnection.OpenAsync();
                await EnsureTable();
            }
            catch (Exception ex)
            {
                internalLogger.Log(ex);
            }
        }

        private static void CreateDatabase()
        {
            var filePath = StudioConstants.LogsFile;

            if (!File.Exists(filePath))
                SQLiteConnection.CreateFile(filePath);
        }

        private Task EnsureTable()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS LOGS(ID INTEGER PRIMARY KEY, Sender VARCHAR, Level VARCHAR, Title VARCHAR, Message VARCHAR, Date DATETIME, CallerFile VARCHAR, CallerMember VARCHAR, CallerLine INTEGER, StackTrace VARCHAR);";

            using SQLiteCommand command = new SQLiteCommand(commandString, dbConnection);

            return command.ExecuteNonQueryAsync();
        }

        private class LogsProcessor
        {
            private readonly DefaultLoggingService loggingService;
            private readonly BlockingCollection<Log> logs;
            private Action<Log> enqueueLog;
            private CancellationTokenSource cancellationToken;
            private Task<bool> processTask;
            private bool isProcessing;

            public LogsProcessor(DefaultLoggingService loggingService)
            {
                this.loggingService = loggingService;
                logs = new BlockingCollection<Log>(new ConcurrentQueue<Log>());
                enqueueLog = log => logs.Add(log);

                loggingService.logManager.Initialized += LogManagerInitialized;
                loggingService.logManager.DumpRequested += LogManagerDumpRequested;
            }

            public void EnqueueLog(Log log)
            {
                enqueueLog(log);
            }

            private void Start()
            {
                if (!loggingService.logManager.IsInitialized || isProcessing)
                    return;
                isProcessing = true;

                enqueueLog = log => logs.Add(log);

                cancellationToken = new CancellationTokenSource();
                processTask = Task.Run(Process, cancellationToken.Token);
                processTask.ContinueWith(t => StopAndDump());
            }

            private void Stop()
            {
                if (!isProcessing)
                    return;
                isProcessing = false;

                enqueueLog = log => loggingService.internalLogger.Log("A log has been queued, but the logs processor has been stopped", log);

                var taskStatus = CreateTaskStatus(processTask);
                loggingService.internalLogger.Log($"The logging processor has stopped with the result: {taskStatus}");

                cancellationToken.Cancel();
                cancellationToken.Dispose();
                cancellationToken = null;

                processTask.Dispose();
                processTask = null;
            }

            private void StopAndDump()
            {
                Stop();

                if (logs.Any())
                    Dump();
            }

            private async Task<bool> Process()
            {
                Log lastDequeuedLog = null;

                try
                {
                    while (CanProcess())
                    {
                        cancellationToken.Token.ThrowIfCancellationRequested();

                        lastDequeuedLog = logs.Take();
                        lastDequeuedLog = loggingService.UpdateLog(lastDequeuedLog);

                        if (!loggingService.CanLog(lastDequeuedLog))
                            continue;

                        await loggingService.InsertIntoDb(lastDequeuedLog).ConfigureAwait(false);
                        loggingService.logManager.OnLogCompleted(lastDequeuedLog);
                    }
                }
                catch (Exception ex)
                {
                    loggingService.internalLogger.Log(ex, lastDequeuedLog);
                    throw;
                }

                return true;
            }

            private bool CanProcess()
            {
                return isProcessing && loggingService.logManager.IsInitialized;
            }

            private void Dump()
            {
                var wasProcessing = isProcessing;
                if (isProcessing)
                    Stop();

                var sb = new StringBuilder();
                sb.AppendLine("Dumping all logs...");
                sb.AppendLine($"{logs.Count} logs remaining in the queue");
                while (logs.Any())
                    sb.AppendLine($"{logs.Take()}");

                loggingService.internalLogger.Log(sb.ToString().Trim());

                if (wasProcessing && !isProcessing)
                    Start();
            }

            private static string CreateTaskStatus(Task<bool> task)
            {
                var properties = task.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var sb = new StringBuilder();
                foreach (var property in properties)
                    sb.AppendLine($"{property.Name}: {TryGetValue(property)}, ");

                return $"{sb}";

                object TryGetValue(PropertyInfo propertyInfo)
                {
                    try
                    {
                        var value = propertyInfo.GetValue(task);
                        return value;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            private void LogManagerInitialized(object sender, EventArgs e)
            {
                Start();
            }

            private void LogManagerDumpRequested(object sender, EventArgs e)
            {
                Dump();
            }
        }
    }

    internal interface ILoggingService
    {
        void EnqueueLog(string sender, string title, string message, string level, string callerFile, string callerMember, int callerLine, string stackTrace);

        Task<DataTable> GetLogs();

        long GetSize();

        Task DeleteAll();
    }
}