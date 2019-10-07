using SLStudio.Logging;
using System;
using System.Collections.Generic;

namespace SLStudio.WinForms.WindowsManager
{
    public static class WindowsManager
    {
        static readonly ILog Log = LogManager.GetLogger(nameof(WindowsManager));

        private static List<ManagedWindow> windows;

        public static void Initialize()
        {
            windows = new List<ManagedWindow>();
        }

        public static void Show(ManagedWindow window, ManagedWindow parent = null)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static void ShowDialog(ManagedWindow window, ManagedWindow parent = null)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static void Register()
        {
            throw new NotImplementedException();
        }

        public static void Flush()
        {
            windows.Clear();
        }
    }
}
