using SLStudio.Properties;
using System;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class frmStartScreen : FormBase
    {
        public frmStartScreen()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            foreach (string path in Settings.Default.recentFilesList)
            {
                Controls.RecentFilesList control = new Controls.RecentFilesList(path);
                control.Dock = DockStyle.Top;
                panelOpen.Controls.Add(control);
            }
        }
      
        #region Events
        private void OnCreateNew(object sender, EventArgs e)
        {

        }

        private void OnOpenProjectOrSolution(object sender, EventArgs e)
        {

        }

        private void OnCheckoutProject(object sender, EventArgs e)
        {

        }

        private void OnTutorials(object sender, EventArgs e)
        {

        }

        private void OnContinueWithoutCode(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion
    }
}
