using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using SLStudio.ViewsExtensions.Themes;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomControls
{
    public partial class CustomRecentFilesList : UserControl, IThemedControl, IMultiLanguageControl
    {
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

        string thisPath;
        IStartScreen parent;

        //Todo: finish this

        public CustomRecentFilesList()
        {
            InitializeComponent();
            UpdateTheme();
            ThemeManager.AddControl(this);
        }

        public CustomRecentFilesList(string path, IStartScreen parent)
        {
            InitializeComponent();
            UpdateTheme();
            ThemeManager.AddControl(this);

            this.parent = parent;

            string filePath = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);

            lblPath.Text = filePath;
            lblName.Text = fileName;

            string fileType = Path.GetExtension(path);
            if(fileType == ".sls")
            {
                picture.Image = Resources.Images.Icons.folder_Closed_32xLG;
            }
            else
                picture.Image = Resources.Images.Icons.application_32xLG;

            lblDate.Text = File.GetLastWriteTime(path).ToString();

            thisPath = path;
        }

        public void UpdateTheme()
        {
            this.BackColor = theme.theme;
            this.ForeColor = theme.font;
        }

        public void UpdateLanguage()
        {
            throw new NotImplementedException();
        }

        private void RecentFilesList_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = theme.selection;
        }

        private void RecentFilesList_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = theme.theme;
        }

        private void LblPath_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
            else
                this.BackColor = theme.selectionDark;
        }

        private void LblPath_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.BackColor = theme.selection;
                this.parent.OpenFromRecentList(thisPath);
            }
        }

        private void OpenDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void RemoveFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
