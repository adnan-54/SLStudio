using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.MainMenu
{
    internal class MainMenuItemTemplateSelector : ItemContainerTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
        {
            if (item is not IMenuItem)
                return base.SelectTemplate(item, parentItemsControl);

            var key = new DataTemplateKey(item.GetType());
            return (DataTemplate)parentItemsControl.FindResource(key);
        }
    }
}
