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
            if (item is IToolPanel)
                return ToolTemplate;

            if (item is IDocumentPanel)
                return DocumentTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}