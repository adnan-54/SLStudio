using SLStudio.Logging;
using System.Windows.Forms;

namespace SLStudio.WinForms
{
    public partial class MainWindow : Form
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
