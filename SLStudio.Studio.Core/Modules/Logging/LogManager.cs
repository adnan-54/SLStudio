using Caliburn.Micro;
using SLStudio.Studio.Core.Modules.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.Studio.Core.Modules.Logging
{
    public static class LogManager
    {
        private static readonly List<Logger> currentLoggers = new List<Logger>();
        private static readonly IOutput output = IoC.Get<IOutput>();
        private static readonly string logFileName = "Logger2.log";
        private static readonly string logDirectory = Path.Combine(Application.StartupPath, "Logger");
        private static readonly string logFullPath = Path.Combine(logDirectory, logFileName);
        private static readonly string logConnectionString = $"Data Source={logFullPath};Version=3;datetimeformat=CurrentCulture";
        private static readonly SQLiteConnection logConnection = new SQLiteConnection(logConnectionString);

        public static ILogger GetLogger(string name)
        {
            Logger logger = currentLoggers.FirstOrDefault(x => x.Name == name);
            if(logger == null)
            {
                logger = new Logger(name);
                currentLoggers.Add(logger);
            }

            return logger;
        }

        internal static void Log(string title, string message, string type, string origin)
        {
            CreateDirectory();
            CreateDatabase();
            InsertIntoLog(title, message, type, origin, DateTime.Now);
        }

        public static DataTable GetLogs()
        {
            DataTable log = new DataTable();

            logConnection.Open();

            try
            {
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM TB_LOGS ORDER BY Date DESC", logConnection))
                {
                    dataAdapter.Fill(log);
                }
            }
            catch { }
            finally
            {
                logConnection.Close();
            }

            return log;
        }

        public static void ClearLog()
        {
            DialogResult confirmationDialog = MessageBox.Show(Resources.LoggerResources.ClearConfirmation, Resources.LoggerResources.Clear, MessageBoxButtons.OKCancel, MessageBoxIcon.Question); ;
            if (confirmationDialog == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(logFullPath))
                        File.Delete(logFullPath);

                    CreateDatabase();
                }
                catch { }
                finally
                {
                    InsertIntoLog("Log cleaned", "Log cleaned successfully", "Info", "LogManager", DateTime.Now);
                }
            }
        }

        public static async Task ExportLogToHtml()
        {
            try
            {
                DataTable log = GetLogs();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html><head><title>Log</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
                sb.AppendLine("<style>");
                sb.AppendLine("html{ margin: 0px; height: 100%; width: 100%; }");
                sb.AppendLine("body{ font-family: \"Trebuchet MS\", Arial, Helvetica, sans-serif; margin: 0px; min-height: 100%; width: 100%; }");
                sb.AppendLine("table{ border-collapse: collapse; width: 100%; }");
                sb.AppendLine("th, td{ text-align: left; border-bottom: 1px solid #ddd; padding: 8px; }");
                sb.AppendLine("tr:nth-child(even){ background-color: #f2f2f2 }");
                sb.AppendLine("tr:hover{ background-color: #ddd; }");
                sb.AppendLine("th{ padding-top: 12px; padding-bottom: 12px; background-color: #4CAF50; color: white; }");
                sb.AppendLine("</style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine("<table>");

                sb.AppendLine("<tr>");
                foreach (DataColumn column in log.Columns)
                {
                    sb.AppendLine($"<th>{column.ColumnName}</th>");
                }
                sb.AppendLine("</tr>");

                foreach (DataRow row in log.Rows)
                {
                    sb.AppendLine("<tr>");
                    foreach (DataColumn column in log.Columns)
                    {
                        sb.AppendLine($"<td>{row[column.ColumnName].ToString()}</td>");
                    }
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</table>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                try
                {
                    using (SaveFileDialog saveFile = new SaveFileDialog())
                    {
                        saveFile.Filter = "html file (*.html)|*.html";
                        saveFile.CheckPathExists = true;
                        saveFile.FileName = $"slstudio_log_{DateTime.Now.ToString().Replace('/', '-').Replace(' ', '_').Replace(':', '-')}";

                        if (saveFile.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamWriter writer = new StreamWriter(saveFile.FileName))
                            {
                                await writer.WriteAsync(sb.ToString());
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    sb.Clear();
                }
            }
            catch { }
        }

        private static void CreateDirectory()
        {
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
        }

        private static void CreateDatabase()
        {
            if (!File.Exists(logFullPath))
            {
                SQLiteConnection.CreateFile(logFullPath);

                CreateTable();
            }
            else
            {
                logConnection.Open();

                try
                {
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = logConnection;

                        command.CommandText = "SELECT * FROM TB_LOGS ORDER BY ROWID ASC LIMIT 1;";

                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    File.Delete(logFullPath);
                    CreateDatabase();
                }
                finally
                {
                    logConnection.Close();
                }
            }
        }

        private static void CreateTable()
        {
            logConnection.Open();

            try
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.Connection = logConnection;

                    command.CommandText = "CREATE TABLE TB_LOGS(ID INTEGER PRIMARY KEY, Type VARCHAR(16), Title VARCHAR(64), Message VARCHAR(1024), Origin VARCHAR(32), Date DATETIME);";

                    command.ExecuteNonQuery();
                }
            }
            catch { }
            finally
            {
                logConnection.Close();
            }
        }

        private static void InsertIntoLog(string title, string message, string type, string origin, DateTime date)
        {
            logConnection.Open();

            try
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.Connection = logConnection;
                    command.CommandText = "INSERT INTO TB_LOGS(Type, Title, Message, Origin, Date) VALUES (@type, @title, @message, @origin, @date);";

                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@origin", origin);
                    command.Parameters.AddWithValue("@date", date);

                    command.ExecuteNonQuery();
                }

                if(string.IsNullOrWhiteSpace(message))
                    LogToOutput($"[{origin}, {date}]: ({type}) {title}");
                else
                    LogToOutput($"[{origin}, {date}]:({type}) {title} - \"{message}\"");
            }
            catch { }
            finally
            {
                logConnection.Close();
            }
        }

        private static void LogToOutput(string message)
        {
            output.AppendLine(message);
        }
    }
}
