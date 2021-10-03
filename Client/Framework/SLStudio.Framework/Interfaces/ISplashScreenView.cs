using System;

namespace SLStudio
{
    public interface ISplashScreenView
    {
        bool IsShowing { get; }

        string Status { get; set; }

        void Show();

        void Close();
    }
}