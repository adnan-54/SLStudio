using MahApps.Metro.Controls;
using System.Windows;

namespace SLStudio.Core
{
    public class DialogWindow : MetroWindow
    {
        public DialogWindow()
        {
            if (WpfHelpers.TryFindDefaultStyle<DialogWindow>(out Style style))
                Style = style;
        }
    }
}