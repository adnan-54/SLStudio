namespace SLStudio
{
    internal class SplashScreenManager : Service, ISplashScreenManager
    {
        private readonly IUiSynchronization uiSynchronization;
        private ISplashScreen splashScreen;

        public SplashScreenManager(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
        }

        public void UpdateStatus(string status)
        {
            UpdateStatusCore(status);
        }

        public void UpdateStatus(string format, params string[] values)
        {
            var status = string.Format(format, values);
            UpdateStatusCore(status);
        }


        public void SetSplashScreen(ISplashScreen splashScreen)
        {
            this.splashScreen = splashScreen;
        }

        public void Close()
        {
            uiSynchronization.Execute(() => splashScreen?.Close());
            splashScreen = null;
        }

        private void UpdateStatusCore(string status)
        {
            uiSynchronization.Execute(() => splashScreen?.UpdateStatus(status));
        }
    }
}
