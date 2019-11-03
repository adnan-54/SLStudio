using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace SLStudio.Studio.Core.Framework.Controls
{
    public abstract class HwndWrapper : HwndHost
    {
        private const string WindowClass = "GraphicsDeviceControlHostWindowClass";
        private IntPtr _hWnd;
        private IntPtr _hWndPrev;
        private bool _applicationHasFocus;
        private bool _mouseInWindow;
        private Point _previousPosition;
        private readonly HwndMouseState _mouseState = new HwndMouseState();
        private bool _isMouseCaptured;

        public event EventHandler<HwndMouseEventArgs> HwndLButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndLButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndLButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndRButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndRButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndRButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndMButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndMButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndMButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndMouseMove;
        public event EventHandler<HwndMouseEventArgs> HwndMouseEnter;
        public event EventHandler<HwndMouseEventArgs> HwndMouseLeave;
        public event EventHandler<HwndMouseEventArgs> HwndMouseWheel;
        public new bool IsMouseCaptured
        {
            get { return _isMouseCaptured; }
        }

        protected HwndWrapper()
        {
            Application.Current.Activated += OnApplicationActivated;
            Application.Current.Deactivated += OnApplicationDeactivated;

            CompositionTarget.Rendering += OnCompositionTargetRendering;

            if (Application.Current.Windows.Cast<Window>().Any(x => x.IsActive))
                _applicationHasFocus = true;
        }

        protected override void Dispose(bool disposing)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;
            if (Application.Current != null)
            {
                Application.Current.Activated -= OnApplicationActivated;
                Application.Current.Deactivated -= OnApplicationDeactivated;
            }

            base.Dispose(disposing);
        }
        public new void CaptureMouse()
        {
            if (_isMouseCaptured)
                return;

            NativeMethods.SetCapture(_hWnd);
            _isMouseCaptured = true;
        }

        public new void ReleaseMouseCapture()
        {
            if (!_isMouseCaptured)
                return;

            NativeMethods.ReleaseCapture();
            _isMouseCaptured = false;
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            var width = (int) ActualWidth;
            var height = (int) ActualHeight;

            if (width < 1 || height < 1)
                return;

            Render(_hWnd);
        }

        protected abstract void Render(IntPtr windowHandle);

        private void OnApplicationActivated(object sender, EventArgs e)
        {
            _applicationHasFocus = true;
        }

        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            _applicationHasFocus = false;
            ResetMouseState();

            if (_mouseInWindow)
            {
                _mouseInWindow = false;
                RaiseHwndMouseLeave(new HwndMouseEventArgs(_mouseState));
            }

            ReleaseMouseCapture();
        }

        private void ResetMouseState()
        {
            bool fireL = _mouseState.LeftButton == MouseButtonState.Pressed;
            bool fireM = _mouseState.MiddleButton == MouseButtonState.Pressed;
            bool fireR = _mouseState.RightButton == MouseButtonState.Pressed;
            bool fireX1 = _mouseState.X1Button == MouseButtonState.Pressed;
            bool fireX2 = _mouseState.X2Button == MouseButtonState.Pressed;
            
            _mouseState.LeftButton = MouseButtonState.Released;
            _mouseState.MiddleButton = MouseButtonState.Released;
            _mouseState.RightButton = MouseButtonState.Released;
            _mouseState.X1Button = MouseButtonState.Released;
            _mouseState.X2Button = MouseButtonState.Released;

            var args = new HwndMouseEventArgs(_mouseState);
            if (fireL)
                RaiseHwndLButtonUp(args);
            if (fireM)
                RaiseHwndMButtonUp(args);
            if (fireR)
                RaiseHwndRButtonUp(args);
            if (fireX1)
                RaiseHwndX1ButtonUp(args);
            if (fireX2)
                RaiseHwndX2ButtonUp(args);

            _mouseInWindow = false;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            _hWnd = CreateHostWindow(hwndParent.Handle);
            return new HandleRef(this, _hWnd);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            NativeMethods.DestroyWindow(hwnd.Handle);
            _hWnd = IntPtr.Zero;
        }

        private IntPtr CreateHostWindow(IntPtr hWndParent)
        {
            RegisterWindowClass();

            return NativeMethods.CreateWindowEx(0, WindowClass, "",
               NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE,
               0, 0, (int) Width, (int) Height, hWndParent, IntPtr.Zero, IntPtr.Zero, 0);
        }

        private void RegisterWindowClass()
        {
            var wndClass = new NativeMethods.WNDCLASSEX();
            wndClass.cbSize = (uint) Marshal.SizeOf(wndClass);
            wndClass.hInstance = NativeMethods.GetModuleHandle(null);
            wndClass.lpfnWndProc = NativeMethods.DefaultWindowProc;
            wndClass.lpszClassName = WindowClass;
            wndClass.hCursor = NativeMethods.LoadCursor(IntPtr.Zero, NativeMethods.IDC_ARROW);

            NativeMethods.RegisterClassEx(ref wndClass);
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_MOUSEWHEEL:
                    if (_mouseInWindow)
                    {
                        int delta = NativeMethods.GetWheelDeltaWParam(wParam.ToInt32());
                        RaiseHwndMouseWheel(new HwndMouseEventArgs(_mouseState, delta, 0));
                    }
                    break;
                case NativeMethods.WM_LBUTTONDOWN:
                    _mouseState.LeftButton = MouseButtonState.Pressed;
                    RaiseHwndLButtonDown(new HwndMouseEventArgs(_mouseState));
                    break;
                case NativeMethods.WM_LBUTTONUP:
                    _mouseState.LeftButton = MouseButtonState.Released;
                    RaiseHwndLButtonUp(new HwndMouseEventArgs(_mouseState));
                    break;
                case NativeMethods.WM_LBUTTONDBLCLK:
                    RaiseHwndLButtonDblClick(new HwndMouseEventArgs(_mouseState, MouseButton.Left));
                    break;
                case NativeMethods.WM_RBUTTONDOWN:
                    _mouseState.RightButton = MouseButtonState.Pressed;
                    RaiseHwndRButtonDown(new HwndMouseEventArgs(_mouseState));
                    break;
                case NativeMethods.WM_RBUTTONUP:
                    _mouseState.RightButton = MouseButtonState.Released;
                    RaiseHwndRButtonUp(new HwndMouseEventArgs(_mouseState));
                    break;
                case NativeMethods.WM_RBUTTONDBLCLK:
                    RaiseHwndRButtonDblClick(new HwndMouseEventArgs(_mouseState, MouseButton.Right));
                    break;
                case NativeMethods.WM_MBUTTONDOWN:
                    _mouseState.MiddleButton = MouseButtonState.Pressed;
                    RaiseHwndMButtonDown(new HwndMouseEventArgs(_mouseState));
                    break;
                case NativeMethods.WM_MBUTTONUP:
                    _mouseState.MiddleButton = MouseButtonState.Released;
                    RaiseHwndMButtonUp(new HwndMouseEventArgs(_mouseState));
                    break;
                case NativeMethods.WM_MBUTTONDBLCLK:
                    RaiseHwndMButtonDblClick(new HwndMouseEventArgs(_mouseState, MouseButton.Middle));
                    break;
                case NativeMethods.WM_XBUTTONDOWN:
                    if (((int) wParam & NativeMethods.MK_XBUTTON1) != 0)
                    {
                        _mouseState.X1Button = MouseButtonState.Pressed;
                        RaiseHwndX1ButtonDown(new HwndMouseEventArgs(_mouseState));
                    }
                    else if (((int) wParam & NativeMethods.MK_XBUTTON2) != 0)
                    {
                        _mouseState.X2Button = MouseButtonState.Pressed;
                        RaiseHwndX2ButtonDown(new HwndMouseEventArgs(_mouseState));
                    }
                    break;
                case NativeMethods.WM_XBUTTONUP:
                    if (((int) wParam & NativeMethods.MK_XBUTTON1) != 0)
                    {
                        _mouseState.X1Button = MouseButtonState.Released;
                        RaiseHwndX1ButtonUp(new HwndMouseEventArgs(_mouseState));
                    }
                    else if (((int) wParam & NativeMethods.MK_XBUTTON2) != 0)
                    {
                        _mouseState.X2Button = MouseButtonState.Released;
                        RaiseHwndX2ButtonUp(new HwndMouseEventArgs(_mouseState));
                    }
                    break;
                case NativeMethods.WM_XBUTTONDBLCLK:
                    if (((int) wParam & NativeMethods.MK_XBUTTON1) != 0)
                        RaiseHwndX1ButtonDblClick(new HwndMouseEventArgs(_mouseState, MouseButton.XButton1));
                    else if (((int) wParam & NativeMethods.MK_XBUTTON2) != 0)
                        RaiseHwndX2ButtonDblClick(new HwndMouseEventArgs(_mouseState, MouseButton.XButton2));
                    break;
                case NativeMethods.WM_MOUSEMOVE:
                    if (!_applicationHasFocus)
                        break;

                    _mouseState.ScreenPosition = PointToScreen(new Point(
                        NativeMethods.GetXLParam((int) lParam),
                        NativeMethods.GetYLParam((int) lParam)));

                    if (!_mouseInWindow)
                    {
                        _mouseInWindow = true;

                        RaiseHwndMouseEnter(new HwndMouseEventArgs(_mouseState));

                        _hWndPrev = NativeMethods.GetFocus();
                        NativeMethods.SetFocus(_hWnd);

                        var tme = new NativeMethods.TRACKMOUSEEVENT
                        {
                            cbSize = Marshal.SizeOf(typeof (NativeMethods.TRACKMOUSEEVENT)),
                            dwFlags = NativeMethods.TME_LEAVE,
                            hWnd = hwnd
                        };
                        NativeMethods.TrackMouseEvent(ref tme);
                    }

                    if (_mouseState.ScreenPosition != _previousPosition)
                        RaiseHwndMouseMove(new HwndMouseEventArgs(_mouseState));

                    _previousPosition = _mouseState.ScreenPosition;

                    break;
                case NativeMethods.WM_MOUSELEAVE:

                    if (_isMouseCaptured)
                        break;

                    ResetMouseState();

                    RaiseHwndMouseLeave(new HwndMouseEventArgs(_mouseState));

                    NativeMethods.SetFocus(_hWndPrev);

                    break;
            }

            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        protected virtual void RaiseHwndLButtonDown(HwndMouseEventArgs args)
        {
            HwndLButtonDown?.Invoke(this, args);
        }

        protected virtual void RaiseHwndLButtonUp(HwndMouseEventArgs args)
        {
            HwndLButtonUp?.Invoke(this, args);
        }

        protected virtual void RaiseHwndRButtonDown(HwndMouseEventArgs args)
        {
            HwndRButtonDown?.Invoke(this, args);
        }

        protected virtual void RaiseHwndRButtonUp(HwndMouseEventArgs args)
        {
            HwndRButtonUp?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMButtonDown(HwndMouseEventArgs args)
        {
            HwndMButtonDown?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMButtonUp(HwndMouseEventArgs args)
        {
            HwndMButtonUp?.Invoke(this, args);
        }

        protected virtual void RaiseHwndLButtonDblClick(HwndMouseEventArgs args)
        {
            HwndLButtonDblClick?.Invoke(this, args);
        }

        protected virtual void RaiseHwndRButtonDblClick(HwndMouseEventArgs args)
        {
            HwndRButtonDblClick?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMButtonDblClick(HwndMouseEventArgs args)
        {
            HwndMButtonDblClick?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMouseEnter(HwndMouseEventArgs args)
        {
            HwndMouseEnter?.Invoke(this, args);
        }

        protected virtual void RaiseHwndX1ButtonDown(HwndMouseEventArgs args)
        {
            HwndX1ButtonDown?.Invoke(this, args);
        }

        protected virtual void RaiseHwndX1ButtonUp(HwndMouseEventArgs args)
        {
            HwndX1ButtonUp?.Invoke(this, args);
        }

        protected virtual void RaiseHwndX2ButtonDown(HwndMouseEventArgs args)
        {
            HwndX2ButtonDown?.Invoke(this, args);
        }

        protected virtual void RaiseHwndX2ButtonUp(HwndMouseEventArgs args)
        {
            HwndX2ButtonUp?.Invoke(this, args);
        }

        protected virtual void RaiseHwndX1ButtonDblClick(HwndMouseEventArgs args)
        {
            HwndX1ButtonDblClick?.Invoke(this, args);
        }

        protected virtual void RaiseHwndX2ButtonDblClick(HwndMouseEventArgs args)
        {
            HwndX2ButtonDblClick?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMouseLeave(HwndMouseEventArgs args)
        {
            HwndMouseLeave?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMouseMove(HwndMouseEventArgs args)
        {
            HwndMouseMove?.Invoke(this, args);
        }

        protected virtual void RaiseHwndMouseWheel(HwndMouseEventArgs args)
        {
            HwndMouseWheel?.Invoke(this, args);
        }
    }
}