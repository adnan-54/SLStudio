using SLStudio.Properties;
using SLStudio.ViewsExtensions.CustomComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class frmStartScreen : FormBase
    {
        public frmStartScreen()
        {
            InitializeComponent();
            SetupForm();
            PopulateRecentFileList();

            #region Language binding
            this.Text = Resources.Forms.frmStartScreen.startScreen;
            this.lblTitle.Text = Resources.Forms.frmStartScreen.label_slstudio;
            this.lblOpenRecent.Text = Resources.Forms.frmStartScreen.label_open_recent;
            this.lblGetSarted.Text = Resources.Forms.frmStartScreen.label_get_started;
            this.lblContinueWithoutCode.Text = Resources.Forms.frmStartScreen.link_continue;
            this.btnCreateNew.Text = Resources.Forms.frmStartScreen.button_create_new;
            this.btnOpenProject.Text = Resources.Forms.frmStartScreen.button_open_solution;
            this.btnClone.Text = Resources.Forms.frmStartScreen.button_clone_checkout;
            this.btnTutorials.Text = Resources.Forms.frmStartScreen.button_tutorials;
            #endregion
        }

        private void PopulateRecentFileList()
        {
            panelOpen.Controls.Clear();

            List<string> toRemoveList = new List<string>();

            foreach (string path in Settings.Default.recentFilesList)
            {
                if (!File.Exists(path))
                    toRemoveList.Add(path);
            }

            foreach (string toRemove in toRemoveList)
            {
                Settings.Default.recentFilesList.Remove(toRemove);
            }

            Settings.Default.Save();

            foreach (string path in Settings.Default.recentFilesList)
            {
                RecentFilesList control = new RecentFilesList(path);
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

        private void OnContinueWithoutCode(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmMain main = new frmMain();
            main.Owner = this;
            main.Show();
            this.Hide();
        }

        private void btnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCloseMouseEnter(object sender, EventArgs e)
        {
            btnClose.FlatAppearance.MouseOverBackColor = Settings.Default.error;
            btnClose.FlatAppearance.MouseDownBackColor = Settings.Default.themeDark;
        }

        private void ButtonOnMouseEnter(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            button.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
        }

        private void DgvOnDataSourceChanged(object sender, EventArgs e)
        {
            PopulateRecentFileList();
        }
        #endregion

        private List<Keys> _code = new List<Keys> { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A };
        private List<Keys> pressedKeys = new List<Keys> { Keys.A, Keys.A, Keys.A, Keys.A, Keys.A, Keys.A, Keys.A, Keys.A, Keys.A, Keys.A };
        private void FrmStartScreen_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.RemoveAt(0);
            pressedKeys.Add(e.KeyCode);

            if (_code.SequenceEqual(pressedKeys))
            {
                MessageBox.Show("KONAMI!!!");
            }
        }
    }
}
