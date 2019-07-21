using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Views.Forms
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            //this.Opacity = 0;
        }

        private void SobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(frmAbout about = new frmAbout())
            {
                about.ShowDialog();
            }
        }

        private void SairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NovoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            /*using (frmSplashScreen splash = new frmSplashScreen())
            {
                splash.Show();
                Thread.Sleep(5000);
                splash.Close();
            }

            this.Opacity = 100;

            using (frmStartScreen startScreen = new frmStartScreen())
            {
                startScreen.ShowDialog();
            }*/
        }
    }
}
