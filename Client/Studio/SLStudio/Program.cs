using System;

namespace SLStudio
{
    internal static class Program
    {
        [STAThread]
        public static int Main()
        {
            var startup = Startup.GetStartup();
            return startup.Start();
        }
    }
}