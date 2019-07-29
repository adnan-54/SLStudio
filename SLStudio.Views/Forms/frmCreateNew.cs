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

namespace Views.Forms
{
    public partial class frmCreateNew : Form
    {
        public Models.Solution solution;

        private string selectedDir;

        public frmCreateNew()
        {
            InitializeComponent();
            txtDirectory.Text = selectedDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            txtSolutionName.Focus();
        }

        private bool IsValid()
        {
            #region solutionNameValidations
            if(true)
            {
                return false;
            }
            #endregion
            #region titleValidations

            #endregion
            #region descriptionValidations

            #endregion
            #region directoryValidations

            #endregion
            #region authorValidations

            #endregion

            return true;
        }

        private void BtnSearchDirectory_Click(object sender, EventArgs e)
        {
            using(FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if(fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    selectedDir = fbd.SelectedPath;
                    txtDirectory.Text = Path.Combine(selectedDir, txtSolutionName.Text);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    solution = Presenters.Solution.Create(txtSolutionName.Text, txtTitle.Text, txtDescription.Text, txtAuthor.Text, txtDirectory.Text);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void TxtSolutionName_TextChanged(object sender, EventArgs e)
        {
            txtDirectory.Text = Path.Combine(selectedDir, txtSolutionName.Text);
            txtDirectory.SelectionStart = txtDirectory.Text.Length;
            txtDirectory.SelectionLength = 0;
            txtDirectory.ScrollToCaret();
        }

        private void TxtDirectory_TextChanged(object sender, EventArgs e)
        {
            txtDirectory.SelectionStart = txtDirectory.Text.Length;
            txtDirectory.SelectionLength = 0;
            txtDirectory.ScrollToCaret();
        }
    }
}
