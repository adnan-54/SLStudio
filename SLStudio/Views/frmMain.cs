using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            if (Settings.Default.showStartScreen)
            {
                frmStartScreen startScreen = new frmStartScreen();

                var dialogResult = startScreen.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {

                }
                else
                if (dialogResult == DialogResult.Yes)
                {

                }
                else
                if (dialogResult == DialogResult.Retry)
                {

                }

                startScreen.Dispose();
            }

            InitializeComponent();
        }

        private void XuiButton1_Click(object sender, EventArgs e)
        {
            frmLogger logger = new frmLogger();
            logger.ShowDialog();
            logger.Dispose();
        }
    }
}
