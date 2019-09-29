using SLStudio.Logging;
using System;
using System.Windows.Forms;

namespace SLStudio.WinForms.Views
{
    public partial class ShellView : Form
    {
        static readonly ILog Log = LogManager.GetLogger(nameof(ShellView));

        public ShellView()
        {
            InitializeComponent();
        }

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogView logView = new LogView();
            logView.ShowDialog(this);
        }

        private void OnAboutViewClick(object sender, EventArgs e)
        {
            AboutView about = new AboutView();
            about.ShowDialog(this);
        }

        private void ModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewSolutionView newSolution = new CreateNewSolutionView();
            newSolution.ShowDialog(this);
        }
    }
}
