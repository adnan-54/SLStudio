namespace SLStudio.Studio.Core.Framework.Menus
{
    public class ExcludeMenuDefinition
    {
        public MenuDefinition MenuDefinitionToExclude { get; }

        public ExcludeMenuDefinition(MenuDefinition menuDefinition)
        {
            MenuDefinitionToExclude = menuDefinition;
        }
    }
}
