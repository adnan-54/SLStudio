using AvalonDock.Layout;
using System.Linq;

namespace SLStudio
{
    public class WorkspaceLayoutUpdateStrategy : ILayoutUpdateStrategy
    {
        private LayoutAnchorablePane leftPane;
        private LayoutAnchorablePane topPane;
        private LayoutAnchorablePane bottomPane;
        private LayoutAnchorablePane rightPane;
        private LayoutDocumentPane documentPane;

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            if (anchorableToShow.Content is not IWorkspaceTool toolItem)
                return false;
            EnsurePanes(layout);

            switch (toolItem.Placement)
            {
                case WorkspacePlacement.Left:
                    leftPane.Children.Add(anchorableToShow);
                    break;
                case WorkspacePlacement.Top:
                    topPane.Children.Add(anchorableToShow);
                    break;
                case WorkspacePlacement.Bottom:
                    bottomPane.Children.Add(anchorableToShow);
                    break;
                case WorkspacePlacement.Right:
                    rightPane.Children.Add(anchorableToShow);
                    break;
                default:
                    documentPane.Children.Add(anchorableToShow);
                    break;
            }

            return true;
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            new WorkspaceToolBehavior().Attach(anchorableShown);
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument doocumentToShow, ILayoutContainer destinationContainer)
        {
            if (doocumentToShow.Content is not IWorkspaceDocument documentItem)
                return false;
            EnsurePanes(layout);

            documentPane.Children.Add(doocumentToShow);

            return true;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument documentShown)
        {
            new WorkspaceDocumentBehavior().Attach(documentShown);
        }

        //todo: try move this to constructor
        private void EnsurePanes(LayoutRoot layout)
        {
            if (leftPane is not null && topPane is not null && bottomPane is not null && rightPane is not null && documentPane is not null)
                return;

            var anchorablePanes = layout.Descendents().OfType<LayoutAnchorablePane>();
            var documentPanes = layout.Descendents().OfType<LayoutDocumentPane>();

            leftPane = anchorablePanes.First(p => p.Name == "LeftPane");
            topPane = anchorablePanes.First(p => p.Name == "TopPane");
            bottomPane = anchorablePanes.First(p => p.Name == "BottomPane");
            rightPane = anchorablePanes.First(p => p.Name == "RightPane");
            documentPane = documentPanes.First();
        }
    }
}
