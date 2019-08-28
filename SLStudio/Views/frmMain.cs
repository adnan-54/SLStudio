using SLStudio.Enums;
using SLStudio.Interfaces;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class frmMain : FormBase
    {
        IStartScreen parent;

        public frmMain()
        {
            InitializeComponent();
            SetupForm();
        }

        public frmMain(IStartScreen parent)
        {
            InitializeComponent();
            SetupForm();
            this.parent = parent;
        }

        public frmMain(IStartScreen parent, StartScreenResponse response, string path)
        {
            InitializeComponent();
            SetupForm();
            this.parent = parent;
        }

        #region methods
        public override void SetupForm()
        {
            base.SetupForm();

            #region Theme bindings
            btnClose.FlatAppearance.MouseOverBackColor = Settings.Default.error;
            btnClose.FlatAppearance.MouseDownBackColor = Settings.Default.style;
            btnMinimize.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnChangeState.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnMinimize.FlatAppearance.MouseDownBackColor = Settings.Default.style;
            btnChangeState.FlatAppearance.MouseDownBackColor = Settings.Default.style;
            #endregion

            #region Language bindings

            #endregion

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region Events
        private void OnButtonCloseClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                if (this.parent != null)
                    parent.CloseFromChild();
                else
                    this.Close();
            }
        }

        private void OnMinimizeClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void OnButtonCloseEnter(object sender, EventArgs e)
        {
            btnClose.FlatAppearance.MouseOverBackColor = Settings.Default.error;
            btnClose.FlatAppearance.MouseDownBackColor = Settings.Default.style;
        }

        private void TitleBarButtonsOnMouseEnter(object sender, EventArgs e)
        {
            btnMinimize.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnChangeState.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnMinimize.FlatAppearance.MouseDownBackColor = Settings.Default.style;
            btnChangeState.FlatAppearance.MouseDownBackColor = Settings.Default.style;
        }

        private void OnButtonChangeStateClick(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }

        }
        #endregion
    }
}
