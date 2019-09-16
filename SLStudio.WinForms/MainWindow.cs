using SLStudio.Logging;
using SLStudio.Models;
using System;
using System.Windows.Forms;

namespace SLStudio.WinForms
{
    public partial class MainWindow : Form
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private Mod mod;
        public Mod CurrentMod
        {
            get
            {
                return mod;
            }
            set
            {
                mod = value;
                this.Text = mod.modName;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void ModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*using(CreateNewModWindow createNew = new CreateNewModWindow(this))
            {
                var dialogResult = createNew.ShowDialog(this);
            }*/
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                if (!backgroundWorker1.CancellationPending)
                {
                    logger.Info("benchmark");
                    backgroundWorker1.ReportProgress(i);
                }
                else
                    e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            dataGridView1.DataSource = LogManager.GetLog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation)
            {
                backgroundWorker1.CancelAsync();
            }

        }
    }
}
