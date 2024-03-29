﻿using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.Docking
{
    public class PanesStyleSelector : StyleSelector
    {
        private readonly Style toolStyle;
        private readonly Style documentStyle;
        private readonly Style fileDocumentStyle;

        public PanesStyleSelector()
        {
            WpfHelpers.TryFindStyle("StudioToolStyle", out toolStyle);
            WpfHelpers.TryFindStyle("StudioDocumentStyle", out documentStyle);
            WpfHelpers.TryFindStyle("StudioFileDocumentStyle", out fileDocumentStyle);
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is IToolItem)
                return toolStyle;
            if (item is IFileDocumentItem)
                return fileDocumentStyle;
            if (item is IDocumentItem)
                return documentStyle;

            return base.SelectStyle(item, container);
        }
    }
}