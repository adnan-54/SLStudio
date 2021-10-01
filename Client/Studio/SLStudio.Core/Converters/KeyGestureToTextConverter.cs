using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace SLStudio.Core.Converters
{
    public class KeyGestureToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not KeyGesture keyGesture)
                return Binding.DoNothing;

            if (keyGesture is MultiKeyGesture multiGesture)
                return $"{multiGesture.DisplayString}";

            return $"{OemKeyGesture.CreateDisplayString(keyGesture.Key, keyGesture.Modifiers)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}