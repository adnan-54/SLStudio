using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Views.Forms
{
    public partial class frmMain : Form
    {
        List<string> filesNames = new List<string>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach(String fn in ofd.FileNames)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(fn);
                        filesNames.Add(fileName);
                        listBox1.Items.Add(fileName);
                        lblFilesSelected.Text = $"Files selected: {filesNames.Count()}";
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
