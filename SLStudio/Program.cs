using SLStudio.Views;
using System;
using System.Windows.Forms;

namespace SLStudio
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            /*CultureInfo newCulture = new CultureInfo(Settings.Default.languageDefault, true);
            newCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;*/

            Logger.Initialize();
            Logger.LogInfo(Resources.Messages.Logger.startingProgram);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainView());
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}