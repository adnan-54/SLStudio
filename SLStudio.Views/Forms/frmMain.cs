using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Views.Util;

namespace Views.Forms
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            frmSplashScreen splashScreen = new frmSplashScreen();
            splashScreen.ShowDialog();
            InitializeComponent();
        }
    }
}
