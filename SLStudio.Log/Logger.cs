using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public static class Logger
    {
        const string logPath = @".\logger\";
        const string logName = logPath + "log.log";

        public static void Log(string message)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            
            if (!File.Exists(logName))
            {
                FileStream fs = File.Create(logName);
                fs.Close();
            }

            using (StreamWriter sw = File.AppendText(logName))
            {
                sw.WriteLine(DateTime.Now + ": " + message);
            }
        }
    }
}
