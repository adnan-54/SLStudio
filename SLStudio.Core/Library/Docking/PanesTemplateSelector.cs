using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.Docking
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate toolTemplate;
        private readonly DataTemplate documentTemplate;
        private readonly DataTemplate fileDocumentTemplate;

        public PanesTemplateSelector()
        {
            WpfHelpers.TryFindResource("StudioToolTemplate", out toolTemplate);
            WpfHelpers.TryFindResource("StudioDocumentTemplate", out documentTemplate);
            WpfHelpers.TryFindResource("StudioFileDocumentTemplate", out fileDocumentTemplate);
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IToolPanel)
                return toolTemplate;
            if (item is IDocumentPanel)
                return documentTemplate;
            if (item is IFileDocumentPanel)
                return fileDocumentTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}