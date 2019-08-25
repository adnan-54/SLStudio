using System;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.Themes
{
    public static class ThemeManager
    {
        public static void Refresh(Form parent)
        {
            try
            {
                foreach (Control children in parent.Controls)
                {
                    if (children.HasChildren)
                        Refresh(children);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private static void Refresh(Control parent)
        {
            try
            {
                foreach (Control children in parent.Controls)
                {
                    if (children.HasChildren)
                        Refresh(children);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}