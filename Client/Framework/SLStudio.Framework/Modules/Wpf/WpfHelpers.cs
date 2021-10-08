using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SLStudio
{
    public static class WpfHelpers
    {
        public static bool TryFindResource<TResource>(object resourceKey, out TResource resource)
            where TResource : class
        {
            resource = Application.Current.TryFindResource(resourceKey) as TResource;
            return resource != null;
        }

        public static bool TryFindDefaultStyle<TControl>(out Style style)
            where TControl : FrameworkElement
        {
            TryFindResource(typeof(TControl), out style);
            return style != null;
        }

        public static IEnumerable<TChild> FindVisualChildren<TChild>(DependencyObject parent)
            where TChild : DependencyObject
        {
            if (parent is null)
                yield break;

            for (int index = 0; index < VisualTreeHelper.GetChildrenCount(parent); index++)
            {
                var child = VisualTreeHelper.GetChild(parent, index);

                if (child is not null and TChild validChild)
                    yield return validChild;

                foreach (var childsChild in FindVisualChildren<TChild>(child))
                    yield return childsChild;
            }
        }
    }
}
