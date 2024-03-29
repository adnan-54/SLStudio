﻿using System.Windows;
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
            if (item is IToolItem)
                return toolTemplate;
            if (item is IFileDocumentItem)
                return fileDocumentTemplate;
            if (item is IDocumentItem)
                return documentTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}