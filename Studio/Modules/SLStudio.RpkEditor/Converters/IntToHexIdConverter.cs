using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SLStudio.RpkEditor.Converters
{
    internal class IntToHexIdConverter : MarkupExtension, IValueConverter
    {
        private static IntToHexIdConverter instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new IntToHexIdConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int id))
                return Binding.DoNothing;

            return id.ToStringId();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}