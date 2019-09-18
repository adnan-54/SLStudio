using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.Logging
{
    public static class LogManager
    {
        public static bool IsInitialized { get; private set; } = false;

        private static List<Logger> loggers = new List<Logger>();

        public static ILog GetLogger(string name)
        {
            if (!IsInitialized)
                Initialize();

            foreach (Logger log in loggers)
            {
                if (log.Name == name)
                    return log;
            }

            Logger newLogger = new Logger(name);
            loggers.Add(newLogger);

            return newLogger;
        }

        public static void Initialize()
        {
            if (IsInitialized)
                return;

            InternalLogManager internalManager = InternalLogManager.GetInstance();

            if (!internalManager.LogDirectoryExists())
                internalManager.CreateLogDirectory();

            if (!internalManager.LogFileExists())
                internalManager.CreateLogFile();

            IsInitialized = true;
        }

        public static DataTable GetLog()
        {
            InternalLogManager internalManager = InternalLogManager.GetInstance();

            return internalManager.GetLog();
        }

        public static async Task ExportLogToHtml()
        {
            InternalLogManager internalManager = InternalLogManager.GetInstance();
            DataTable log = internalManager.GetLog();

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
            catch(Exception ex)
            {
                internalManager.InsertIntoLog("Error", ex.ToString(), ex.Message, "LogManager", DateTime.Now);
            }
            finally
            {
                sb.Clear();
            }
        }

        public static void ClearLog()
        {
            InternalLogManager internalManager = InternalLogManager.GetInstance();

            internalManager.ClearLog();
        }
    }
}
