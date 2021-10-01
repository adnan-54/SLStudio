using System;
using System.Globalization;
using System.Windows.Data;

namespace SLStudio.Core.Converters
{
    public class EqualityConverter : IValueConverter
    {
        public ComparisonMode Comparison { get; set; }

        public bool Negate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!double.TryParse(value?.ToString(), out var leftOperand) || !double.TryParse(parameter?.ToString(), out var rightOperand))
                return Binding.DoNothing;

            bool result = Comparison switch
            {
                ComparisonMode.EqualTo => leftOperand == rightOperand,
                ComparisonMode.GreaterOrEqualTo => leftOperand >= rightOperand,
                ComparisonMode.GreaterThan => leftOperand > rightOperand,
                ComparisonMode.LessOrEqualTo => leftOperand <= rightOperand,
                ComparisonMode.LessThan => leftOperand < rightOperand,
                ComparisonMode.NotEqualTo => leftOperand != rightOperand,
                _ => false
            };

            if (Negate)
                result = !result;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public enum ComparisonMode
        {
            LessThan,
            LessOrEqualTo,
            GreaterThan,
            GreaterOrEqualTo,
            EqualTo,
            NotEqualTo
        }
    }
}