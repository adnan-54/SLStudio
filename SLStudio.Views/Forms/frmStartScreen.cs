using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Views.Util;

namespace Views.Forms
{
    public partial class frmStartScreen : MetroFramework.Forms.MetroForm
    {
        public Models.Project project;

        public frmStartScreen()
        {
            InitializeComponent();
        }

        private void MetroTile3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void MetroTile2_Click(object sender, EventArgs e)
        {
            try
            {
                project = Presenters.Project.Open();
                this.DialogResult = DialogResult.Yes;
            }
            catch(Exception ex)
            {
                ShowMessage.Exception(ex);
            }
        }

        private void MetroTile1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
