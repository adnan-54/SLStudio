using Newtonsoft.Json;
using SLStudio.Util;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SLStudio
{
    public static class Logger
    {
        private static string logPath = Path.Combine(Application.StartupPath, "logger");
        private static string logName = "loggerJson.log";
        private static string logPathName = Path.Combine(logPath, logName);

        enum LogType { Error, Warning, Info, Other }

        public static void LogError(Exception exception)
        {
            LogError(exception.Message, $"{exception.Source}");
        }

        public static void LogError(string message, string description = "")
        {
            Log(message, description, LogType.Error);
        }

        public static void LogWarning(string message, string description = "")
        {
            Log(message, description, LogType.Warning);
        }

        public static void LogInfo(string message, string description = "")
        {
            Log(message, description, LogType.Info);
        }

        public static void Initialize()
        {
            Create();
        }

        public static async void Export()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "html file (*.html)|*.html";
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.FileName = "Log.html";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LogInfo("Log successfuly exported", $"Location: {saveFileDialog.FileName}");

                    string savePath = saveFileDialog.FileName;

                    if (!File.Exists(savePath))
                        File.Create(savePath).Close();

                    string table = Converters.DataTableToHTML(Get());

                    string html = Properties.Resources.logHtmlPage;
                    html = html.Replace("**table**", table);


                    using (var sw = new StreamWriter(savePath))
                    {
                        await sw.WriteAsync(html);
                    }
                }
            }
        }

        public static DataTable Get()
        {
            DataTable log = new DataTable("Logger");

            Create();

            string inputFile = File.ReadAllText(logPathName);

            log = (DataTable)JsonConvert.DeserializeObject(inputFile, (typeof(DataTable)));

            return log;
        }

        public static void Clear()
        {
            DataTable logger = new DataTable("Logger");

            logger.Columns.Add("Id", typeof(int));
            logger.Columns.Add("Type", typeof(string));
            logger.Columns.Add("Message", typeof(string));
            logger.Columns.Add("Description", typeof(string));
            logger.Columns.Add("Date", typeof(DateTime));

            logger.Rows.Add(1, LogType.Info, "Log file cleaned successfully", "", DateTime.Now);

            string outputFile = JsonConvert.SerializeObject(logger);

            File.WriteAllText(logPathName, outputFile);
        }

        private static void Create()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            if (!File.Exists(logPathName) || new System.IO.FileInfo(logPathName).Length < 5)
            {
                DataTable logger = new DataTable("Logger");

                logger.Columns.Add("Id", typeof(int));
                logger.Columns.Add("Type", typeof(string));
                logger.Columns.Add("Message", typeof(string));
                logger.Columns.Add("Description", typeof(string));
                logger.Columns.Add("Date", typeof(DateTime));
               
                logger.Rows.Add(1, LogType.Info, "Log file created successfully", $"Location: {logPathName}", DateTime.Now);

                string outputFile = JsonConvert.SerializeObject(logger);

                File.WriteAllText(logPathName, outputFile);
            }
        }

        private static void Log(string message, string description, LogType type)
        {
            DataTable logger = Get();
            int nextId = logger.Rows.Count + 1;

            logger.Rows.Add(nextId, type, message, description, DateTime.Now);

            string outputFile = JsonConvert.SerializeObject(logger);

            File.WriteAllText(logPathName, outputFile);
        }
    }
}
