using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.Properties;
using SLStudio.ViewsExtensions.CustomControls;
using SLStudio.ViewsExtensions.Language;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class StartScreenView : CustomBorderLessForm, IStartScreen, IThemedControl, IMultiLanguageControl
    {
        public StartScreenView()
        {
            InitializeComponent();
            SetupForm();

            PopulateRecentFileList();

            UpdateTheme();
            ThemeManager.AddControl(this);

            UpdateLanguage();
            LanguageManager.AddControl(this);
        }

        #region IThemedControl, IMultiLanguageControl
        private Theme theme = new Theme(DefaultThemes.UserDefault);
        public Theme Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
            }
        }

        public void UpdateTheme()
        {
            btnTutorials.BackColor = theme.themeDark;
            btnTutorials.ForeColor = theme.font;
            btnTutorials.FlatAppearance.MouseDownBackColor = theme.selection;
            btnTutorials.FlatAppearance.MouseOverBackColor = theme.themeLight;

            btnClone.BackColor = theme.themeDark;
            btnClone.ForeColor = theme.font;
            btnClone.FlatAppearance.MouseDownBackColor = theme.selection;
            btnClone.FlatAppearance.MouseOverBackColor = theme.themeLight;

            btnCreateNew.BackColor = theme.themeDark;
            btnCreateNew.ForeColor = theme.font;
            btnCreateNew.FlatAppearance.MouseDownBackColor = theme.selection;
            btnCreateNew.FlatAppearance.MouseOverBackColor = theme.themeLight;

            btnOpenProject.BackColor = theme.themeDark;
            btnOpenProject.ForeColor = theme.font;
            btnOpenProject.FlatAppearance.MouseDownBackColor = theme.selection;
            btnOpenProject.FlatAppearance.MouseOverBackColor = theme.themeLight;

            lblContinueWithoutCode.ActiveLinkColor = theme.selectionLight;
            lblContinueWithoutCode.LinkColor = Theme.link;

            this.BackColor = theme.theme;
            this.ForeColor = theme.font;
        }

        public void UpdateLanguage()
        {
            this.Text = Resources.Forms.frmStartScreen.startScreen;
            this.lblTitle.Text = Resources.Forms.frmStartScreen.label_slstudio;
            this.lblOpenRecent.Text = Resources.Forms.frmStartScreen.label_open_recent;
            this.lblGetSarted.Text = Resources.Forms.frmStartScreen.label_get_started;
            this.lblContinueWithoutCode.Text = Resources.Forms.frmStartScreen.link_continue;
            this.btnCreateNew.Text = Resources.Forms.frmStartScreen.button_create_new;
            this.btnOpenProject.Text = Resources.Forms.frmStartScreen.button_open_solution;
            this.btnClone.Text = Resources.Forms.frmStartScreen.button_clone_checkout;
            this.btnTutorials.Text = Resources.Forms.frmStartScreen.button_tutorials;
        }
        #endregion

        #region methods
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
                CustomRecentFilesList control = new CustomRecentFilesList(path, this);
                control.Dock = DockStyle.Top;
                panelOpen.Controls.Add(control);
            }
        }

        public void OpenFromRecentList(string path)
        {
            
        }
        #endregion

        #region Events
        private void OnCreateNew(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void OnOpenProjectOrSolution(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void OnCheckoutProject(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void OnTutorials(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void OnContinueWithoutCode(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void ButtonCloseOnClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Application.Exit();
        }
        #endregion
    }
}
