namespace SLStudio.Studio.Core.Framework.ToolBars
{
    public class ExcludeToolBarItemDefinition
    {
        public ToolBarItemDefinition ToolBarItemDefinitionToExclude { get; }

        public ExcludeToolBarItemDefinition(ToolBarItemDefinition ToolBarItemDefinition)
        {
            ToolBarItemDefinitionToExclude = ToolBarItemDefinition;
        }
    }
}