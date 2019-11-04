using SLStudio.Studio.Core.Framework.ToolBars;

namespace SLStudio.Studio.Core.Modules.ToolBars
{
    public interface IToolBarBuilder
    {
        void BuildToolBars(IToolBars result);
        void BuildToolBar(ToolBarDefinition toolBarDefinition, IToolBar result);
    }
}