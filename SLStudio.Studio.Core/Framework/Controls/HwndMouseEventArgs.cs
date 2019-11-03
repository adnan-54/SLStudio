using System;
using System.Windows;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Framework.Controls
{
    public class HwndMouseEventArgs : EventArgs
    {
        public MouseButtonState LeftButton { get; private set; }

        public MouseButtonState RightButton { get; private set; }

        public MouseButtonState MiddleButton { get; private set; }

        public MouseButtonState X1Button { get; private set; }

        public MouseButtonState X2Button { get; private set; }

        public MouseButton? DoubleClickButton { get; private set; }

        public int WheelDelta { get; private set; }

        public int HorizontalWheelDelta { get; private set; }

        public Point ScreenPosition { get; private set; }

        public Point GetPosition(UIElement relativeTo)
        {
            return relativeTo.PointFromScreen(ScreenPosition);
        }

        public HwndMouseEventArgs(HwndMouseState state)
        {
            LeftButton = state.LeftButton;
            RightButton = state.RightButton;
            MiddleButton = state.MiddleButton;
            X1Button = state.X1Button;
            X2Button = state.X2Button;
            ScreenPosition = state.ScreenPosition;
        }

        public HwndMouseEventArgs(HwndMouseState state, int mouseWheelDelta, int mouseHWheelDelta)
            : this(state)
        {
            WheelDelta = mouseWheelDelta;
            HorizontalWheelDelta = mouseHWheelDelta;
        }
        
        public HwndMouseEventArgs(HwndMouseState state, MouseButton doubleClickButton) : this(state)
        {
            DoubleClickButton = doubleClickButton;
        }
    }
}
