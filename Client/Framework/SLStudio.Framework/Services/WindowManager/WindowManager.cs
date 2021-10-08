using System;
using System.Windows;

namespace SLStudio
{
    internal class WindowManager : IWindowManager
    {
        private readonly IApplicationInfo applicationInfo;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IObjectFactory objectFactory;
        private readonly IViewLocator viewLocator;

        public WindowManager(IApplicationInfo applicationInfo, IUiSynchronization uiSynchronization, IObjectFactory objectFactory, IViewLocator viewLocator)
        {
            this.applicationInfo = applicationInfo;
            this.uiSynchronization = uiSynchronization;
            this.objectFactory = objectFactory;
            this.viewLocator = viewLocator;
        }

        TViewModel IWindowManager.Show<TViewModel>()
        {
            Create<TViewModel>(out var view, out var viewModel);

            uiSynchronization.Execute(() => view.Show());

            return viewModel;
        }

        TViewModel IWindowManager.ShowDialog<TViewModel>()
        {
            Create<TViewModel>(out var view, out var viewModel);

            uiSynchronization.Execute(() => view.ShowDialog());

            return viewModel;
        }

        private void Create<TViewModel>(out Window view, out TViewModel viewModel) where TViewModel : class, IWindowViewModel
        {
            viewModel = objectFactory.Create<TViewModel>();
            var viewType = viewLocator.LocateWindow<TViewModel>();
            view = CreateWindow(viewModel, viewType);
        }

        private Window CreateWindow(IWindowViewModel viewModel, Type viewType)
        {
            Window view = null;

            uiSynchronization.Execute(() =>
            {
                view = Activator.CreateInstance(viewType) as Window;
                view.DataContext = viewModel;
                var currentWindow = applicationInfo.CurrentWindow;
                if (currentWindow != view)
                    view.Owner = currentWindow;

                AttachBehavior(view);
            });

            return view;
        }

        private void AttachBehavior(Window view)
        {
            var behavior = new WindowBehavior(uiSynchronization);
            behavior.Attach(view);
        }
    }
}