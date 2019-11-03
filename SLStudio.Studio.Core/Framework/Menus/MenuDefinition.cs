using SLStudio.Studio.Core.Framework.Commands;
using System;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Framework.Menus
{
    public class MenuDefinition : MenuDefinitionBase
    {
        private readonly int _sortOrder;
        private readonly string _text;

        public MenuBarDefinition MenuBar { get; }

        public override int SortOrder
        {
            get { return _sortOrder; }
        }

        public override string Text
        {
            get { return _text; }
        }

        public override Uri IconSource
        {
            get { return null; }
        }

        public override KeyGesture KeyGesture
        {
            get { return null; }
        }

        public override CommandDefinitionBase CommandDefinition
        {
            get { return null; }
        }

        public MenuDefinition(MenuBarDefinition menuBar, int sortOrder, string text)
        {
            MenuBar = menuBar;
            _sortOrder = sortOrder;
            _text = text;
        }
    }
}