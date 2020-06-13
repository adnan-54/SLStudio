using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.Docking
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ToolTemplate { get; set; }

        public DataTemplate DocumentTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ITool)
                return ToolTemplate;

            if (item is IDocument)
                return DocumentTemplate;

            return base.SelectTemplate(item, container);
        }
    }

    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }

        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ITool)
                return ToolStyle;

            if (item is IDocument)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}