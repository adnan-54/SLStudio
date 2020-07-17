using SLStudio.Logging;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace SLStudio.Core
{
    public static class ViewBinder
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(ViewBinder));

        private static readonly ContentPropertyAttribute DefaultContentProperty = new ContentPropertyAttribute("Content");

        public static DependencyProperty BindProperty = DependencyProperty.RegisterAttached("Bind", typeof(object), typeof(ViewBinder), new PropertyMetadata(OnModelChanged));

        public static void SetBind(DependencyObject d, object value)
        {
            d.SetValue(BindProperty, value);
        }

        public static object GetBind(DependencyObject d)
        {
            return d.GetValue(BindProperty);
        }

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue || e.NewValue == null)
                return;

            var viewType = ViewLocator.LocateView(e.NewValue);
            var viewInstance = (FrameworkElement)Activator.CreateInstance(viewType);
            viewInstance.DataContext = e.NewValue;
            SetContentProperty(d, viewInstance);
        }

        private static bool SetContentProperty(object targetLocation, object view)
        {
            if (view is FrameworkElement fe && fe.Parent != null)
                SetContentPropertyCore(fe.Parent, null);

            return SetContentPropertyCore(targetLocation, view);
        }

        private static bool SetContentPropertyCore(object targetLocation, object view)
        {
            try
            {
                var type = targetLocation.GetType();
                var contentProperty = type.GetCustomAttributes(typeof(ContentPropertyAttribute), true).OfType<ContentPropertyAttribute>().FirstOrDefault() ?? DefaultContentProperty;
                type.GetProperty(contentProperty?.Name ?? DefaultContentProperty.Name).SetValue(targetLocation, view, null);

                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }
    }
}