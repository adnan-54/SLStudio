namespace SLStudio.Core
{
    class SplashScreenService : ISplashScreenService
    {
        public void Close()
        {

        }

        public void Show()
        {

        }

        public void UpdateStatus(string status)
        {

        }
    }

    public interface ISplashScreenService
    {
        void Show();
        void Close();
        void UpdateStatus(string status);
    }
}
