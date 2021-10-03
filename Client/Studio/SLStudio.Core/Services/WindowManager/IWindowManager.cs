using System;

namespace SLStudio.Core
{
    public interface IWindowManager
    {
        void ShowWindow(object model, Type viewType = null);

        void ShowWindow<T>() where T : class, IWindow;

        bool? ShowDialog(object model, Type viewType = null);

        bool? ShowDialog<T>() where T : class, IWindow;
    }
}