using System;
using System.Windows;

namespace SLStudio
{
    internal class SplashScreen : StudioService, ISplashScreen
    {
        private readonly IUiSynchronization uiSynchronization;
        private ISplashScreenView view;

        public SplashScreen(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
        }

        public bool IsShowing => view is not null && view.IsShowing;

        public string Status => view?.Status;

        public void Show()
        {
            if (view is null || IsShowing)
                return;

            uiSynchronization.Execute(() => view.Show());
        }

        public void SetView(ISplashScreenView view)
        {
            this.view = view;
        }

        public void UpdateStatus(string status)
        {
            if (view is null || !IsShowing)
                return;

            uiSynchronization.Execute(() => view.Status = status);
        }

        public void UpdateStatus(string format, params object[] args)
        {
            UpdateStatus(string.Format(format, args));
        }

        public void Close()
        {
            if (!IsShowing || view is null)
                return;

            uiSynchronization.Execute(() => view.Close());

            view = null;
        }
    }
}