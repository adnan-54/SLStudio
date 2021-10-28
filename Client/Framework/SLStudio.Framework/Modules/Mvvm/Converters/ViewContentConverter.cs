using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace SLStudio
{
    internal class ViewContentConverter : MarkupExtension, IValueConverter
    {
        private static readonly ViewContentConverter instance = new();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IViewModel viewModel)
                return IoC.Get<IViewFactory>()?.Create<UserControl>(viewModel);
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
