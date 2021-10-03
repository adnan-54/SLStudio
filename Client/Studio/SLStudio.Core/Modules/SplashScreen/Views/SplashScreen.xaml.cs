using SLStudio.Core.Modules.SplashScreen.Resources;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio.Core.Modules.SplashScreen
{
    public partial class SplashScreen : Window, ISplashScreen
    {
        public SplashScreen()
        {
            InitializeComponent();
            versionText.Text = string.Format(SplashScreenResources.Version, StudioConstants.ProductVersion);
        }

        public void UpdateStatus(string status)
        {
            Dispatcher.BeginInvoke(() => statusText.Text = status);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}