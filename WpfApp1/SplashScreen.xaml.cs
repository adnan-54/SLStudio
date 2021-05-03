using SLStudio;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISplashScreenView
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        public bool IsShowing { get; private set; }

        public string Status 
        {
            get => StatusText.Text;
            set => StatusText.Text = value;
        }

        void ISplashScreenView.Show()
        {
            IsShowing = true;
            Show();
        }

        void ISplashScreenView.Close()
        {
            IsShowing = false;
            Close();
        }
    }
}
