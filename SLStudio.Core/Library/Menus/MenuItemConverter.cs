using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SLStudio.Core.Menus
{
    public class MenuItemConverter : MarkupExtension, IValueConverter
    {
        private static MenuItemConverter instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
            => instance ??= new MenuItemConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is MenuItemTypes menuType))
                return false;

            return menuType switch
            {
                MenuItemTypes.Separator => value is IMenuSeparatorItem,
                MenuItemTypes.Toggle => value is IMenuToggleItem,
                MenuItemTypes.Normal => value is IMenuItem,
                _ => false,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public enum MenuItemTypes
    {
        Normal,
        Toggle,
        Separator
    }
}