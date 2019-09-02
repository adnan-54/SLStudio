using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomMinimizeButton : UserControl
    {
        private CustomBorderLessForm parent;
        public CustomBorderLessForm ParentForm
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public CustomMinimizeButton()
        {
            InitializeComponent();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && parent != null)
            {
                parent.WindowState = FormWindowState.Minimized;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {

        }

        private void OnMouseEnter(object sender, EventArgs e)
        {

        }

        private void OnMouseLeave(object sender, EventArgs e)
        {

        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
