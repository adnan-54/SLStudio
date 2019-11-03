namespace SLStudio.Studio.Core.Framework.ToolBars
{
    public class ToolBarDefinition
    {
        public int SortOrder { get; }

        public string Name { get; }

        public ToolBarDefinition(int sortOrder, string name)
        {
            SortOrder = sortOrder;
            Name = name;
        }
    }
}