namespace SLStudio.Core
{
    public interface ISplashScreen
    {
        void Show();

        void UpdateStatus(string status);

        void Close();
    }
}