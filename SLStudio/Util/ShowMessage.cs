using System;
using System.Windows.Forms;

namespace SLStudio.Util
{
    public static class ShowMessage
    {
        public static DialogResult Error(Exception exception)
        {
            return MessageBox.Show(exception.Message, exception.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
