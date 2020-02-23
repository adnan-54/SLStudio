using Caliburn.Micro;
using System.Windows;

namespace SLStudio.Core.Modules.SplashScreen.ViewModels
{
    internal class SplashScreenViewModel : Screen, ISplashScreen
    {
        public async void Close()
        {
            await TryCloseAsync();
        }

        public void UpdateStatus(string status)
        {
            MessageBox.Show(status);
        }
    }
}
