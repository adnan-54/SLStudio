using AvalonDock.Layout;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core
{
    public class LayoutInitializer : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            if (anchorableToShow.Content is IToolItem tool)
            {
                var target = $"{tool.Placement}";
                var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == target);
                if (toolsPane == null)
                {
                    switch (tool.Placement)
                    {
                        case WorkspaceItemPlacement.Left:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Horizontal, target, InsertPosition.Start);
                            break;

                        case WorkspaceItemPlacement.Right:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Horizontal, target, InsertPosition.End);
                            break;

                        case WorkspaceItemPlacement.Bottom:
                            toolsPane = CreateAnchorablePane(layout, Orientation.Vertical, target, InsertPosition.End);
                            break;
                    }
                }

                toolsPane.Children.Add(anchorableToShow);

                return true;
            }

            return false;
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            if (anchorableShown.Content is IToolItem tool)
            {
                if (anchorableShown.Parent is LayoutAnchorablePane anchorablePane && anchorablePane.ChildrenCount == 1)
                {
                    switch (tool.Placement)
                    {
                        case WorkspaceItemPlacement.Left:
                        case WorkspaceItemPlacement.Right:
                            anchorablePane.DockWidth = new GridLength(tool.Width, GridUnitType.Pixel);
                            anchorableShown.AutoHideWidth = tool.Width * 0.75;
                            break;

                        case WorkspaceItemPlacement.Bottom:
                            anchorablePane.DockHeight = new GridLength(tool.Height, GridUnitType.Pixel);
                            anchorableShown.AutoHideHeight = tool.Height * 0.75;
                            break;
                    }
                }
            }
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
        }

        private static LayoutAnchorablePane CreateAnchorablePane(LayoutRoot layout, Orientation orientation, string paneName, InsertPosition position)
        {
            var parent = layout.Descendents().OfType<LayoutPanel>().First(d => d.Orientation == orientation);
            var toolsPane = new LayoutAnchorablePane { Name = paneName };
            if (position == InsertPosition.Start)
                parent.InsertChildAt(0, toolsPane);
            else
                parent.Children.Add(toolsPane);
            return toolsPane;
        }

        private enum InsertPosition
        {
            Start,
            End
        }
    }
}