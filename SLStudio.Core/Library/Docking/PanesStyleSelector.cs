using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.Docking
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }

        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is IToolPanel)
                return ToolStyle;

            if (item is IDocumentPanel)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}