using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SLStudio
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {

            #region Updated recent files list
            List<string> toRemoveList = new List<string>(); 

            foreach (string path in Settings.Default.recentFilesList)
            {
                if (!File.Exists(path))
                    toRemoveList.Add(path);
            }

            foreach(string toRemove in toRemoveList)
            {
                Settings.Default.recentFilesList.Remove(toRemove);
            }

            Settings.Default.Save();
            #endregion

            Logger.Initialize();
            Logger.LogInfo("Starting program", DateTime.Now.ToString());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Views.frmMain());
        }
    }
}
