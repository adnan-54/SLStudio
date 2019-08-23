﻿using SLStudio.Properties;
using SLStudio.Views.CustomComponents;
using SLStudio.Views.Themes;
using System;
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
