namespace SLStudio.Core
{
    internal interface ISplashScreen
    {
        string CurrentModule { get; set; }

        void Close();
    }
}