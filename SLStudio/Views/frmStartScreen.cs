using SLStudio.Enums;
using SLStudio.Interfaces;
using SLStudio.Properties;
using SLStudio.ViewsExtensions.CustomComponents;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class frmStartScreen : FormBase, IStartScreen, IThemedWindow, IMultiLanguageWindow
    {
        private Theme theme;
        public Theme Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
                UpdateTheme();
            }
        }

        public frmStartScreen()
        {
            InitializeComponent();
            SetupForm();
            PopulateRecentFileList();
            UpdateTheme();
            UpdateLanguage();
        }

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
                RecentFilesList control = new RecentFilesList(path, this);
                control.Dock = DockStyle.Top;
                panelOpen.Controls.Add(control);
            }
        }

        public void OpenFromRecentList(string path)
        {
            
        }

        public void CloseFromChild()
        {
            this.Close();
        }

        public void UpdateTheme()
        {
            btnCreateNew.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            btnCreateNew.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnOpenProject.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            btnOpenProject.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnClone.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            btnClone.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
            btnTutorials.FlatAppearance.MouseDownBackColor = Settings.Default.selection;
            btnTutorials.FlatAppearance.MouseOverBackColor = Settings.Default.themeLight;
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

        #region Events
        private void OnCreateNew(object sender, EventArgs e)
        {
            frmMain main = new frmMain(this, StartScreenResponse.CreateNew, "");
            main.Owner = this;
            main.Show();
            this.Hide();
        }

        private void OnOpenProjectOrSolution(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "SLStudio solution (*.sls)|*.sls", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    frmMain main = new frmMain(this, StartScreenResponse.Open, ofd.FileName);
                    main.Owner = this;
                    main.Show();
                    this.Hide();
                }
            }
        }

        private void OnCheckoutProject(object sender, EventArgs e)
        {
            frmMain main = new frmMain(this, StartScreenResponse.Clone, "");
            main.Owner = this;
            main.Show();
            this.Hide();
        }

        private void OnTutorials(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void OnContinueWithoutCode(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmMain main = new frmMain(this);
            main.Owner = this;
            main.Show();
            this.Hide();
        }
        #endregion

        #region Konami Code
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
        #endregion
    }
}
