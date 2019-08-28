using SLStudio.Interfaces;
using SLStudio.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.CustomComponents
{
    public partial class RecentFilesList : UserControl
    {
        string thisPath;
        IStartScreen parent;

        public RecentFilesList()
        {
            InitializeComponent();
        }

        public RecentFilesList(string path, IStartScreen parent)
        {
            InitializeComponent();

            this.parent = parent;

            this.BackColor = Settings.Default.theme;
            this.ForeColor = Settings.Default.font;

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

        private void RecentFilesList_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Settings.Default.selection;
        }

        private void RecentFilesList_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Settings.Default.theme;
        }

        private void LblPath_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
            else
                this.BackColor = Settings.Default.selectionDark;
        }

        private void LblPath_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackColor = Settings.Default.selection;
            this.parent.OpenFromRecentList(thisPath);
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
