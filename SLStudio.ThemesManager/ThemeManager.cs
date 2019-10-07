using SLStudio.Logging;
using System;

namespace SLStudio.ThemesManager
{
    public static class ThemeManager
    {
        static readonly ILog Log = LogManager.GetLogger(nameof(ThemeManager));

        public static Theme CurrentTheme;

        public static void Initialize()
        {

        }

        public static void CreteNewTheme()
        {
            try
            {

            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
