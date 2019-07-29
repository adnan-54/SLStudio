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
        private Models.Solution _solution;
        public Models.Solution Solution
        {
            get
            {
                return _solution;
            }
            set
            {
                _solution = value;
                this.Text = "SLStudio - " + value.GetName();
            }
        }

        public frmMain()
        {
            using(frmSplashScreen splashScreen = new frmSplashScreen())
            {
                splashScreen.ShowDialog();
            }

            InitializeComponent();
        }

        private void CreateNew(object sender, EventArgs e)
        {
            try
            {
                using(frmCreateNew frm = new frmCreateNew())
                {
                    if(frm.ShowDialog() == DialogResult.OK)
                    {
                        Solution = frm.solution;
                    }
                }
            }
            catch(Exception ex)
            {
                ShowMessage.Exception(ex);
            }
        }
    }
}
