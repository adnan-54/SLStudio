using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.Logging
{
    internal class InternalLogManager
    {
        private readonly string LogDirectory;
        private readonly string LogFileName;
        private readonly string LogConnectionString;
        private SQLiteConnection LogConnection;

        static InternalLogManager Instance { get; set; } = null;

        private InternalLogManager()
        {
            LogDirectory = Path.Combine(Application.StartupPath, "Logger");
            LogFileName = "Logger.log";

            LogConnectionString = $"Data Source={Path.Combine(LogDirectory, LogFileName)};Version=3;datetimeformat=CurrentCulture";
        }

        public static InternalLogManager GetInstance()
        {
            if (Instance == null)
                Instance = new InternalLogManager();

            return Instance;
        }

        public bool LogDirectoryExists()
        {
            return Directory.Exists(LogDirectory);
        }

        public void CreateLogDirectory()
        {
            Directory.CreateDirectory(LogDirectory);
        }

        public bool LogFileExists()
        {
            return File.Exists(Path.Combine(LogDirectory, LogFileName));
        }

        public void CreateLogFile()
        {
            SQLiteConnection.CreateFile(Path.Combine(LogDirectory, LogFileName));
            CreateLogTable();
        }

        private void CreateLogTable()
        {
            LogConnection = new SQLiteConnection(LogConnectionString);
            LogConnection.Open();

            using (SQLiteCommand command = new SQLiteCommand())
            {
                command.Connection = LogConnection;
                command.CommandText = "CREATE TABLE IF NOT EXISTS TB_LOGS" +
                                      "("+
                                         "Id INTEGER PRIMARY KEY,"+
                                         "Type VARCHAR(10)," +
                                         "Title VARCHAR(50)," +
                                         "Message VARCHAR(1024)," +
                                         "Origin VARCHAR(256)," +
                                         "Date DATETIME"+
                                      ");";

                command.ExecuteNonQuery();
            }

            LogConnection.Close();
        }

        public void InsertIntoLog(string type, string title, string message, string origin, DateTime date)
        {
            CreateLogTable();

            LogConnection = new SQLiteConnection(LogConnectionString);
            LogConnection.Open();

            using (SQLiteCommand command = new SQLiteCommand())
            {
                command.Connection = LogConnection;
                command.CommandText = "INSERT INTO TB_LOGS(Type, Title, Message, Origin, Date) VALUES (@type, @title, @message, @origin, @date);";

                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@origin", origin);
                command.Parameters.AddWithValue("@date", date);


                command.ExecuteNonQuery();
            }

            LogConnection.Close();
        }

        public DataTable GetLog()
        {
            DataTable log = new DataTable();

            LogConnection = new SQLiteConnection(LogConnectionString);
            LogConnection.Open();

            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM TB_LOGS ORDER BY Date DESC", LogConnection))
            {
                dataAdapter.Fill(log);
            }

            LogConnection.Close();

            return log;
        }

        public void ClearLog()
        {
            LogConnection = new SQLiteConnection(LogConnectionString);
            LogConnection.Open();

            using (SQLiteCommand command = new SQLiteCommand())
            {
                command.Connection = LogConnection;
                command.CommandText = "DROP TABLE TB_LOGS;";

                command.ExecuteNonQuery();
            }

            LogConnection.Close();

            InsertIntoLog("Info", "Log cleaned", "Log cleaned successfully", "", DateTime.Now);
        }
    }
}
