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
    }
}
