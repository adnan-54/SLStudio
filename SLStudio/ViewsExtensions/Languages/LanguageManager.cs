using SLStudio.Extensions.Interfaces;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace SLStudio.ViewsExtensions.Language
{
    public static class LanguageManager
    {
        public static List<IMultiLanguageControl> MultiLanguageControls = new List<IMultiLanguageControl>();

        public static bool AddControl(IMultiLanguageControl control)
        {
            try
            {
                MultiLanguageControls.Add(control);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            return true;
        }
       
        public static bool UpdateControls()
        {
            try
            {
                UpdateMultiLanguageControlsList();

                foreach(IMultiLanguageControl control in MultiLanguageControls)
                {
                    control.UpdateLanguage();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }

        public static bool ApplyLanguage(string language = "en-us")
        {
            try
            {
                UpdateMultiLanguageControlsList();

                CultureInfo newCulture = new CultureInfo(language, true);
                newCulture.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;

                Settings.Default.languageDefault = language;
                Settings.Default.Save();

                UpdateControls();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }

        private static bool UpdateMultiLanguageControlsList()
        {
            try
            {
                foreach (IMultiLanguageControl control in MultiLanguageControls)
                {
                    if (control == null)
                        MultiLanguageControls.Remove(control);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }
    }
}
