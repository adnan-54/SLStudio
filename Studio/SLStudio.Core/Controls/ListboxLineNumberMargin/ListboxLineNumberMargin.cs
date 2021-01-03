using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SLStudio.Core.Controls
{
    public class ListboxLineNumberMargin : MetadataAwareMargin
    {
        private double emSize;
        private Typeface typeface;
        private int maxLineNumberLength = 2;
        private Thickness margin;

        static ListboxLineNumberMargin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListboxLineNumberMargin), new FrameworkPropertyMetadata(typeof(ListboxLineNumberMargin)));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            typeface = CreateTypeface();
            emSize = (double)GetValue(TextBlock.FontSizeProperty);
            margin = (Thickness)GetValue(MarginProperty);
            var text = CreateText(new string('9', maxLineNumberLength));
            return new Size(text.Width + margin.Left + margin.Right, text.Height + margin.Top + margin.Bottom);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Control == null)
                return;

            foreach (var element in GetVisibleElements())
            {
                var index = IndexOf(element);
                if (index >= 0)
                {
                    var text = CreateText((index + 1).ToString(CultureInfo.CurrentCulture));
                    var point = element.TransformToAncestor(Control).Transform(new Point(0, 0));
                    drawingContext.DrawText(text, new Point(RenderSize.Width - text.Width, point.Y));
                }
            }
        }

        protected override void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Control == null)
                return;

            var count = Control.ItemsSource is IList ? (Control.ItemsSource as IList).Count : Control.Items.Count;
            var newLength = count.ToString(CultureInfo.CurrentCulture).Length;

            //The margin looks too small when there is only one digit, so always reserve space for at leat two digits.
            if (newLength < 2)
                newLength = 2;

            if (newLength != maxLineNumberLength)
            {
                maxLineNumberLength = newLength;
                InvalidateMeasure();
            }
            base.CollectionChanged(sender, e);
        }

        private FormattedText CreateText(string text)
        {
            return new FormattedText(text,
                                     CultureInfo.CurrentCulture,
                                     FlowDirection.LeftToRight,
                                     typeface,
                                     emSize,
                                     (Brush)GetValue(System.Windows.Controls.Control.ForegroundProperty));
        }

        private Typeface CreateTypeface()
        {
            var element = this;
            return new Typeface((FontFamily)element.GetValue(TextBlock.FontFamilyProperty),
                                (FontStyle)element.GetValue(TextBlock.FontStyleProperty),
                                (FontWeight)element.GetValue(TextBlock.FontWeightProperty),
                                (FontStretch)element.GetValue(TextBlock.FontStretchProperty));
        }
    }
}