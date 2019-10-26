using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SLStudio.WinForms.WindowsManager
{
    public class WindowsManager : IWindowManager
    {
        private readonly IList<Type> windows;

        public WindowsManager()
        {
            windows = new List<Type>();
        }
    }

    public interface IWindowManager
    {
        
    }

    public interface IManagedWindow
    {
        void Show();
        void Show(IWin32Window owner);
        void ShowDialog();
        void ShowDialog(IWin32Window owner);
    }
}
