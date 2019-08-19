using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SLStudio.Views.Controls
{
    public partial class RecentFilesList : UserControl
    {
        public RecentFilesList(string path)
        {
            InitializeComponent();

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
        }

        private void RecentFilesList_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Themes.Light.Default.styleBorders;
        }

        private void RecentFilesList_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Themes.Light.Default.theme;
        }
    }
}
