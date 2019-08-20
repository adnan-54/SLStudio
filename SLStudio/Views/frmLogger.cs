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
    public partial class frmLogger : MetroFramework.Forms.MetroForm
    {
        public frmLogger()
        {
            InitializeComponent();
        }

        private async void OnClear(object sender, EventArgs e)
        {
            if(MetroFramework.MetroMessageBox.Show(this, "Are you sure you want to clear the logger?", "Clear logger", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Logger.Clear();
                    dataTableView.DataSource = await Task.Run(() => UpdateDataTable());
                }
                catch(Exception ex)
                {
                    Logger.LogError(ex);
                }
            }
        }

        private async void OnExport(object sender, EventArgs e)
        {
            try
            {
                Logger.Export();
                dataTableView.DataSource = await Task.Run(() => UpdateDataTable());
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private async void OnLoad(object sender, EventArgs e)
        {
            try
            {
                dataTableView.DataSource = await Task.Run(() => UpdateDataTable());
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataTable UpdateDataTable()
        {
            return Logger.Get();
        }
    }
}
