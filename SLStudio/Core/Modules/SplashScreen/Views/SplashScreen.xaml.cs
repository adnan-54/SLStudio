using MahApps.Metro.Controls;

namespace SLStudio.Core.Modules.SplashScreen.Views
{
    public partial class SplashScreen : MetroWindow
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        public void UpdateStatus(string status)
        {
            if (string.IsNullOrEmpty(status.Trim()))
                statusText.Text = Modules.SplashScreen.Resources.SplashScreen.Initializing;
            else
                statusText.Text = status;
        }
    }
}