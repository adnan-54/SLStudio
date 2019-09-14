using SLStudio.Logging;
using SLStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            logger.Info("message", "title");

            dataGridView1.DataSource = LogManager.GetLog();
        }

        private void ModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*using(CreateNewModWindow createNew = new CreateNewModWindow(this))
            {
                var dialogResult = createNew.ShowDialog(this);
            }*/
        }
    }
}
