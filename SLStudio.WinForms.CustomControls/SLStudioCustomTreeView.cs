using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SLStudio.WinForms.CustomControls
{
    public class SLStudioCustomTreeView : TreeView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(this.Handle, "explorer", null);
        }
    }
}
