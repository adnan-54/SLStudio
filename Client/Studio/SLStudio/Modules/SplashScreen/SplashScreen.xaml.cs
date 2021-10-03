using System.Windows;

namespace SLStudio
{
    public partial class SplashScreen : Window, ISplashScreen
    {
        public SplashScreen()
        {
            InitializeComponent();

            VersionText.Text = SharedConstants.ProductVersion.ToString(3);
        }

        void ISplashScreen.UpdateStatus(string status)
        {
            StatusText.Text = status;
        }

        void ISplashScreen.Close()
        {
            Close();
        }
    }
}