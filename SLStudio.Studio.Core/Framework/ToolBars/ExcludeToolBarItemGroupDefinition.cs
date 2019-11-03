namespace SLStudio.Studio.Core.Framework.ToolBars
{
    public class ExcludeToolBarItemGroupDefinition
    {
        public ToolBarItemGroupDefinition ToolBarItemGroupDefinitionToExclude { get; }

        public ExcludeToolBarItemGroupDefinition(ToolBarItemGroupDefinition toolBarItemGroupDefinition)
        {
            ToolBarItemGroupDefinitionToExclude = toolBarItemGroupDefinition;
        }
    }
}