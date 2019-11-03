namespace SLStudio.Studio.Core.Framework.Menus
{
    public abstract class MenuItemDefinition : MenuDefinitionBase
    {
        private readonly int _sortOrder;

        public MenuItemGroupDefinition Group { get; }

        public override int SortOrder
        {
            get { return _sortOrder; }
        }

        protected MenuItemDefinition(MenuItemGroupDefinition group, int sortOrder)
        {
            Group = group;
            _sortOrder = sortOrder;
        }
    }
}