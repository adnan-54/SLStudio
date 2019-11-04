using SLStudio.Studio.Core.Framework;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Studio.Core.Modules.Shell.Controls
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ToolTemplate
        {
            get;
            set;
        }

        public DataTemplate DocumentTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ITool)
                return ToolTemplate;

            if (item is IDocument)
                return DocumentTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}