using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.Views
{
    public partial class LoggerView : MetroFramework.Forms.MetroForm
    {
        public LoggerView()
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
