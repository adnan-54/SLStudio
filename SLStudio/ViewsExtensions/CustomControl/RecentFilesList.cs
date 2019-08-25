using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomComponents
{
    public partial class RecentFilesList : UserControl
    {
        public string thisPath;

        public RecentFilesList()
        {
            InitializeComponent();
        }

        public RecentFilesList(string path)
        {
            InitializeComponent();

            //this.BackColor = ThemeManager.styleTheme;
            //this.ForeColor = ThemeManager.styleSelected;

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

            lblDate.Text = File.GetLastWriteTime(path).ToString();

            thisPath = path;
        }

        private void RecentFilesList_MouseEnter(object sender, EventArgs e)
        {
            //this.BackColor = ThemeManager.styleBase;
        }

        private void RecentFilesList_MouseLeave(object sender, EventArgs e)
        {
            //this.BackColor = ThemeManager.styleTheme;
        }

        private void LblPath_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
            //this.BackColor = ThemeManager.styleSelected;
        }

        private void LblPath_MouseUp(object sender, MouseEventArgs e)
        {
            //this.BackColor = ThemeManager.styleBase;
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
