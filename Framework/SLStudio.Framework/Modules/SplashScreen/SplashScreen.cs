using System;
using System.Windows;

namespace SLStudio
{
    internal class SplashScreen : StudioService, ISplashScreen
    {
        private readonly IUiSynchronization uiSynchronization;
        private Type viewType;
        private ISplashScreenView view;
        private bool isShowing;

        public SplashScreen(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
        }

        public string Status => view?.Status;

        public void Show()
        {
            if (viewType is null || isShowing)
                return;

            uiSynchronization.Execute(() =>
            {
                var window = (Window)Activator.CreateInstance(viewType);
                window.Show();

                view = (ISplashScreenView)window;
            });

            isShowing = true;
        }

        public void SetView<TView>() where TView : Window, ISplashScreenView
        {
            viewType = typeof(TView);
        }

        public void UpdateStatus(string status)
        {
            if (view is null || !isShowing)
                return;

            uiSynchronization.Execute(() => view.Status = status);
        }

        public void UpdateStatus(string format, params object[] args)
        {
            UpdateStatus(string.Format(format, args));
        }

        public void Close()
        {
            if (!isShowing || view is null)
                return;

            var window = (Window)view;
            uiSynchronization.Execute(() => window.Close());

            view = null;

            isShowing = false;
        }
    }
}