namespace SLStudio
{
    public interface ISplashScreenManager : IService
    {
        void UpdateStatus(string status);

        void UpdateStatus(string format, params string[] values);

        void SetSplashScreen(ISplashScreen splashScreen);

        void Close();
    }
}
