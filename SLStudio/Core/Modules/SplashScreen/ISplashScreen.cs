namespace SLStudio.Core
{
    public interface ISplashScreen
    {
        string Status { get; set; }

        void Close();
    }
}