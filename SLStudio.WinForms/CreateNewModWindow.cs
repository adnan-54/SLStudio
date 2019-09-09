using SLStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLStudio.WinForms
{
    public partial class CreateNewModWindow : Form
    {
        MainWindow parent;

        public CreateNewModWindow(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void OnButtonCancelClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Close();
        }

        private void OnButtonOkClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Mod mod = new Mod();
                mod.modName = this.TextBoxModName.Text;
                mod.modDescription = this.TextBoxModDescription.Text;
                parent.CurrentMod = mod;
                this.Close();
            }
        }
    }
}
