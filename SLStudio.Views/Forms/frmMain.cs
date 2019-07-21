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
using Views.Util;

namespace Views.Forms
{
    public partial class frmMain : Form
    {
        private Models.Project project;

        public Models.Project Project
        {
            get
            {
                return project;
            }
            set
            {
                project = value;
                this.Text = $"SLStudio - {project.name}";
            }
        }

        public frmMain()
        {
            frmSplashScreen splashScreen = new frmSplashScreen();
            splashScreen.ShowDialog();
            InitializeComponent();
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            using (frmStartScreen startScreen = new frmStartScreen())
            {
                startScreen.ShowDialog();
                if(startScreen.DialogResult == DialogResult.Yes)
                {
                    this.Project = startScreen.project;
                }
            }
        }
    }
}
