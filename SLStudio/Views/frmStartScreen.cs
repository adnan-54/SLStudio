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
            #endregion

            InitializeComponent();
            SetupForm();
            PopulateRecentFileList();
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

        Theme dale;

        Random random;

        int r, g, b;

        Color color;

        #region Events
        private void OnCreateNew(object sender, EventArgs e)
        {
            random = new Random();
            r = random.Next(0, 255);
            b = random.Next(0, 255);
            b = random.Next(0, 255);
            color = Color.FromArgb(r,g,b);
        }

        private void OnOpenProjectOrSolution(object sender, EventArgs e)
        {
            dale = new Theme();
            dale.Borders = color;
        }

        private void OnCheckoutProject(object sender, EventArgs e)
        {
            Theme = dale;
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
