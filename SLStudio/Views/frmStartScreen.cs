using SLStudio.Properties;
using SLStudio.ViewsExtensions.CustomComponents;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class frmStartScreen : FormBase
    {
        public frmStartScreen()
        {
            InitializeComponent();
            SetupForm();

            #region Updated recent files list
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

            PopulateRecentFileList();
            #endregion

            var cult = Thread.CurrentThread.CurrentCulture;
            var cultUi  = Thread.CurrentThread.CurrentUICulture;


            #region Language binding
            this.Text = Resources.frmStartScreen.startScreen;
            this.lblTitle.Text = Resources.frmStartScreen.label_slstudio;
            this.lblOpenRecent.Text = Resources.frmStartScreen.label_open_recent;
            this.lblGetSarted.Text = Resources.frmStartScreen.label_get_started;
            this.lblContinueWithoutCode.Text = Resources.frmStartScreen.link_continue;
            
            this.btnCreateNew.Text = Resources.frmStartScreen.button_create_new;
            this.btnOpenProject.Text = Resources.frmStartScreen.button_open_solution;
            this.btnClone.Text = Resources.frmStartScreen.button_clone_checkout;
            this.btnTutorials.Text = Resources.frmStartScreen.button_tutorials;
            #endregion
        }

        private void PopulateRecentFileList()
        {
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
            this.Theme = new Theme(Enums.DefaultThemes.Dark);
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

        private void btnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCloseMouseEnter(object sender, EventArgs e)
        {
            btnClose.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnClose.FlatAppearance.MouseDownBackColor = Settings.Default.error;
        }
        #endregion
    }
}
