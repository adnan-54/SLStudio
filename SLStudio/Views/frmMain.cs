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
    public partial class frmMain : FormBase
    {
        public frmMain()
        {
            InitializeComponent();
            SetupForm();
        }

        private void XuiButton1_Click(object sender, EventArgs e)
        {
            frmLogger logger = new frmLogger();
            logger.ShowDialog();
            logger.Dispose();
        }
    }
}
