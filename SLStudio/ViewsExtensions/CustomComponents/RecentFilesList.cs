using SLStudio.Views.Themes;
using System;
using System.IO;
using System.Windows.Forms;

namespace SLStudio.Views.CustomComponents
{
    public partial class RecentFilesList : UserControl
    {
        public string thisPath;

        private ThemesManager themeManager = new ThemesManager();

        public ThemesManager ThemeManager
        {
            get
            {
                return themeManager;
            }
            set
            {
                themeManager = value;
            }
        }

        public RecentFilesList(string path)
        {
            InitializeComponent();

            this.BackColor = ThemeManager.styleTheme;
            this.ForeColor = ThemeManager.styleSelected;

            string filePath = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);

            lblPath.Text = filePath;
            lblName.Text = fileName;

            string fileType = Path.GetExtension(path);
            if(fileType == ".sls")
            {
                picture.Image = Resources.Icons.folder_Closed_32xLG;
            }
            else
                picture.Image = Resources.Icons.application_32xLG;

            thisPath = path;
        }

        private void RecentFilesList_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = ThemeManager.styleBase;
        }

        private void RecentFilesList_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = ThemeManager.styleTheme;
        }

        private void LblPath_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = ThemeManager.styleSelected;
        }

        private void LblPath_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackColor = ThemeManager.styleBase;
        }
    }
}
