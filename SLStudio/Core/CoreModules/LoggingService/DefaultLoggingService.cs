using Caliburn.Micro;
using SLStudio.Core.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Core.CoreModules.LoggingService
{
    internal class DefaultLoggingService : ILoggingService
    {
        private static readonly object @lock = new object();
        private readonly SQLiteConnection dbConnection;
        private readonly IEventAggregator eventAggregator;
        private readonly string applicationPath;
        private readonly string logFileName;
        private readonly string logFilePath;
        private readonly string connectionString;

        public DefaultLoggingService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logFileName = "logs.db";
            logFilePath = Path.Combine(applicationPath, "logs", logFileName);
            connectionString = $"Data Source={logFilePath}; Version=3; datetimeformat=CurrentCulture";

            dbConnection = new SQLiteConnection(connectionString);
        }

        public async void Log(NewLogRequestedEvent log)
        {
            await Task.Run(() =>
            {
                lock (@lock)
                {
                    try
                    {
                        LogToDb(log);
                        eventAggregator.PublishOnUIThread(log);
                    }
                    catch (Exception ex)
                    {
                        LogToSimleFile(ex);
                    }

                }
            });
        }

        public DataTable GetLogs()
        {
            DataTable dataTable = new DataTable();

            try
            {
                dbConnection.Open();
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM TB_LOGS ORDER BY Date DESC", dbConnection))
                {

                    lock (@lock)
                        dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogToSimleFile(ex);
            }
            finally
            {
                dbConnection.Close();
            }

            return dataTable;
        }

        public void ExportLogsToHtml(string path)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                var logs = GetLogs();

                stringBuilder.AppendLine("<!DOCTYPE html>");
                stringBuilder.AppendLine("<html><head><title>Log</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
                stringBuilder.AppendLine("<style>");
                stringBuilder.AppendLine("html{ margin: 0px; height: 100%; width: 100%; }");
                stringBuilder.AppendLine("body{ font-family: \"Trebuchet MS\", Arial, Helvetica, sans-serif; margin: 0px; min-height: 100%; width: 100%; }");
                stringBuilder.AppendLine("table{ border-collapse: collapse; width: 100%; }");
                stringBuilder.AppendLine("th, td{ text-align: left; border-bottom: 1px solid #ddd; padding: 8px; }");
                stringBuilder.AppendLine("tr:nth-child(even){ background-color: #f2f2f2 }");
                stringBuilder.AppendLine("tr:hover{ background-color: #ddd; }");
                stringBuilder.AppendLine("th{ padding-top: 12px; padding-bottom: 12px; background-color: #4A92FF; color: white; }");
                stringBuilder.AppendLine("</style>");
                stringBuilder.AppendLine("</head>");
                stringBuilder.AppendLine("<body>");
                stringBuilder.AppendLine("<table>");

                stringBuilder.AppendLine("<tr>");
                foreach (DataColumn column in logs.Columns)
                {
                    stringBuilder.AppendLine($"<th>{column.ColumnName}</th>");
                }
                stringBuilder.AppendLine("</tr>");

                foreach (DataRow row in logs.Rows)
                {
                    stringBuilder.AppendLine("<tr>");
                    foreach (DataColumn column in logs.Columns)
                    {
                        stringBuilder.AppendLine($"<td>{row[column.ColumnName].ToString()}</td>");
                    }
                    stringBuilder.AppendLine("</tr>");
                }

                stringBuilder.AppendLine("</table>");
                stringBuilder.AppendLine("</body>");
                stringBuilder.AppendLine("</html>");

                File.WriteAllText(path, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                LogToSimleFile(ex);
            }
            finally
            {
                stringBuilder.Clear();
            }
        }

        public void ClearAllLogs()
        {
            try
            {
                dbConnection.Open();
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandText = "DROP TABLE TB_LOGS;";

                    command.ExecuteNonQuery();
                }
                EnsureLogTableIsValid();
            }
            catch (Exception ex)
            {
                LogToSimleFile(ex);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private void LogToDb(NewLogRequestedEvent log)
        {
            EnsureDbIsValid();
            InsertLog(log.Sender, log.Level, log.Title, log.Message, log.Date);
        }

        private void EnsureDbIsValid()
        {
            if (!File.Exists(logFilePath))
            {
                EnsureDbFileExists();
                EnsureLogTableIsValid();
            }
            else
            {
                try
                {
                    dbConnection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = dbConnection;
                        command.CommandText = "SELECT * FROM TB_LOGS ORDER BY ROWID ASC LIMIT 1;";
                        command.ExecuteNonQuery();
                    }
                    dbConnection.Close();
                }
                catch
                {
                    dbConnection.Close();
                    File.Delete(logFilePath);
                    EnsureDbIsValid();
                }
            }
        }

        private void EnsureDbFileExists()
        {
            var directoryPath = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!File.Exists(logFilePath))
                SQLiteConnection.CreateFile(logFilePath);
        }

        private void EnsureLogTableIsValid()
        {
            try
            {
                dbConnection.Open();
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandText = "SELECT * FROM TB_LOGS ORDER BY ROWID ASC LIMIT 1;";
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = dbConnection;
                        command.CommandText = "CREATE TABLE TB_LOGS(ID INTEGER PRIMARY KEY, Sender VARCHAR(32), Level VARCHAR(16), Title VARCHAR(128), Message VARCHAR(8192), Date DATETIME);";
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    LogToSimleFile(ex);
                }

            }
            finally
            {
                dbConnection.Close();
            }
        }

        private void InsertLog(string sender, string level, string title, string message, DateTime date)
        {
            try
            {
                dbConnection.Open();
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandText = "INSERT INTO TB_LOGS(Sender, Level, Title, Message, Date) VALUES (@sender, @level, @title, @message, @date);";

                    command.Parameters.AddWithValue("@sender", sender);
                    command.Parameters.AddWithValue("@level", level);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@date", date);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogToSimleFile(ex);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private void LogToSimleFile(Exception exception)
        {
            var path = Path.GetDirectoryName(logFilePath);
            var simpleLogPath = Path.Combine(path, "log.txt");

            File.AppendAllLines(simpleLogPath, GetExceptionDetails(exception));
        }

        private IEnumerable<string> GetExceptionDetails(Exception exception)
        {
            yield return "---------------------------------------";
            yield return DateTime.Now.ToString();
            yield return exception.ToString();
            yield return exception.Message;
            yield return exception.StackTrace;
            yield return exception.TargetSite.Name;
        }
    }
}
