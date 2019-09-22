using SLStudio.Logging;
using System.Reflection;
using System.Windows.Forms;

namespace SLStudio.WinForms
{
    public partial class MainWindow : Form
    {
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
