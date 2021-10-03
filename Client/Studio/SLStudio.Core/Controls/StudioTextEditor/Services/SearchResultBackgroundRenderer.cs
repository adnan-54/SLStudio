using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Linq;
using System.Windows.Media;

namespace SLStudio.Core
{
    internal class SearchResultBackgroundRenderer : IBackgroundRenderer
    {
        private readonly SearchCache cache;
        private readonly Brush markerBrush;
        private readonly Pen markerPen;
        private bool isShowing;

        public SearchResultBackgroundRenderer(SearchCache cache)
        {
            this.cache = cache;
            WpfHelpers.TryFindResource("TextSearchMatchBackground", out markerBrush);
            markerPen = new(markerBrush, 1);

            Results = new();
        }

        public KnownLayer Layer => KnownLayer.Selection;

        public TextSegmentCollection<SearchResult> Results { get; }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (!isShowing || cache.Count == 0 || !textView.VisualLinesValid)
                return;

            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
                return;

            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

            foreach (var result in cache.Where(r => r.StartOffset >= viewStart && r.EndOffset <= viewEnd))
            {
                var geoBuilder = new BackgroundGeometryBuilder
                {
                    AlignToWholePixels = true,
                    BorderThickness = markerPen != null ? markerPen.Thickness : 0,
                    CornerRadius = 3,
                };
                geoBuilder.AddSegment(textView, result);

                var geometry = geoBuilder.CreateGeometry();

                if (geometry != null)
                    drawingContext.DrawGeometry(markerBrush, markerPen, geometry);
            }
        }

        public void Show()
        {
            isShowing = true;
        }

        public void Hide()
        {
            isShowing = false;
        }
    }
}