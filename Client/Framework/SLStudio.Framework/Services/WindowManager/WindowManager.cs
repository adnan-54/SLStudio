using System.Windows;

namespace SLStudio
{
    internal class WindowManager : IWindowManager
    {
        private readonly IViewModelFactory viewModelFactory;
        private readonly IViewFactory viewFactory;
        private readonly IUiSynchronization uiSynchronization;

        public WindowManager(IViewModelFactory viewModelFactory, IViewFactory viewFactory, IUiSynchronization uiSynchronization)
        {
            this.viewModelFactory = viewModelFactory;
            this.viewFactory = viewFactory;
            this.uiSynchronization = uiSynchronization;
        }

        TViewModel IWindowManager.Show<TViewModel>()
        {
            (var view, var viewModel) = Create<TViewModel>();
            uiSynchronization.Execute(() => view.Show());
            return viewModel;
        }

        TViewModel IWindowManager.ShowDialog<TViewModel>()
        {
            (var view, var viewModel) = Create<TViewModel>();
            uiSynchronization.Execute(() => view.ShowDialog());
            return viewModel;
        }

        private (Window window, TViewModel viewModel) Create<TViewModel>()
            where TViewModel : class, IWindowViewModel
        {
            var viewModel = viewModelFactory.Create<TViewModel>();
            var view = viewFactory.Create<Window>(viewModel);

            return (view, viewModel);
        }
    }
}