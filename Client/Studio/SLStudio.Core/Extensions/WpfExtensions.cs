using System.Windows.Media;

namespace SLStudio.Core
{
    public static class WpfExtensions
    {
        public static T GetVisualChild<T>(this Visual source) where T : Visual
        {
            Visual child = null;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
            {
                child = VisualTreeHelper.GetChild(source, i) as Visual;
                if (child != null && child is T)
                    break;
                else if (child != null)
                {
                    child = GetVisualChild<T>(child);
                    if (child != null && child is T)
                        break;
                }
            }
            return child as T;
        }
    }
}