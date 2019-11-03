namespace SLStudio.Studio.Core.Framework.Menus
{
    public class MenuItemGroupDefinition
    {
        public MenuDefinitionBase Parent { get; }

        public int SortOrder { get; }

        public MenuItemGroupDefinition(MenuDefinitionBase parent, int sortOrder)
        {
            Parent = parent;
            SortOrder = sortOrder;
        }
    }
}