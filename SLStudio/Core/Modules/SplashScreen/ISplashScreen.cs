namespace SLStudio.Core.Modules.SplashScreen
{
    internal interface ISplashScreen
    {
        string CurrentModule { get; set; }
        void Close();
    }
}
