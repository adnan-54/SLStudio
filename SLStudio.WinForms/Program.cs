﻿using SLStudio.WinForms.Views;
using System;
using System.Windows.Forms;

namespace SLStudio.WinForms
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShellView());
        }
    }
}
