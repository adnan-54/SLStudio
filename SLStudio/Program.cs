﻿using SLStudio.Properties;
using SLStudio.Views;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace SLStudio
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            CultureInfo newCulture = new CultureInfo(Settings.Default.languageDefault, true);
            newCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            Logger.Initialize();
            Logger.LogInfo(Resources.Messages.Logger.startingProgram);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(Settings.Default.showStartScreen)
            {
                Application.Run(new StartScreenView());
            }
            else
            {
                Application.Run(new MainView());
            }
        }
    }
}