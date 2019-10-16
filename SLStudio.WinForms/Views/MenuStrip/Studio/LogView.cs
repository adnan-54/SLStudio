using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.WinForms.Views
{
    public partial class LogView : Form
    {
        private DataTable LogSource { get; set; }

        public LogView()
        {
            InitializeComponent();

            backgroundWorker1.RunWorkerAsync();
        }

        private void ButtonOkOnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void ButtonExportOnClick(object sender, EventArgs e)
        {
            await LogManager.ExportLogToHtml();
        }

        private void ButtonClearOnClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the log?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LogManager.ClearLog();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void BackgroundWorkDoWork(object sender, DoWorkEventArgs e)
        {
            LogSource = LogManager.GetLog();
        }

        private void BackgroundWorkerWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.DataSource = LogSource;
        }
    }
}
