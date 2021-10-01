using MahApps.Metro.Controls;
using System.Windows;

namespace SLStudio.Core
{
    public class StudioWindow : MetroWindow
    {
        public StudioWindow()
        {
            if (WpfHelpers.TryFindDefaultStyle<StudioWindow>(out Style style))
                Style = style;
        }
    }
}