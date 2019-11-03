using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SLStudio.Studio.Core.Framework.Controls
{
    public class SliderEx : Slider
    {
        [Category("Behavior")]
        public event DragStartedEventHandler ThumbDragStarted;

        [Category("Behavior")]
        public event DragCompletedEventHandler ThumbDragCompleted;

        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);

            base.OnThumbDragStarted(e);
        }

        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            ThumbDragCompleted?.Invoke(this, e);

            base.OnThumbDragCompleted(e);
        }
    }
}
