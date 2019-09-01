using SLStudio.Extensions.Interfaces;
using System;
using System.Collections.Generic;

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

        public static bool Update()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }
    }
}
