using SLStudio.Framework.Modules.Mvvm.Views;
using SLStudio.Logging;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    internal class ViewFactory : IViewFactory
    {
        private static readonly ILogger logger = LogManager.GetLogger<ViewFactory>();

        private readonly IViewLocator viewLocator;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IUiSynchronization uiSynchronization;
        private DefaultView defaultView;
        private DefaultWindow defaultWindow;

        public ViewFactory(IViewLocator viewLocator, IViewModelFactory viewModelFactory, IUiSynchronization uiSynchronization)
        {
            this.viewLocator = viewLocator;
            this.viewModelFactory = viewModelFactory;
            this.uiSynchronization = uiSynchronization;
        }

        TView IViewFactory.Create<TView>(IViewModel viewModel)
        {
            var viewType = typeof(TView);
            if (viewType.IsAssignableTo(typeof(Window)) && viewModel is IWindowViewModel windowViewModel)
                return CreateWindow(windowViewModel) as TView;
            if (viewType.IsAssignableTo(typeof(Control)))
                return CreateView(viewModel) as TView;

            throw new InvalidOperationException($"Could not create a view for type '{viewType.Name}'");
        }

        TView IViewFactory.Create<TView>(Type viewModelType)
        {
            var viewModel = viewModelFactory.Create(viewModelType);
            return ((IViewFactory)this).Create<TView>(viewModel);
        }

        TView IViewFactory.Create<TView, TViewModel>()
        {
            return ((IViewFactory)this).Create<TView>(typeof(TViewModel));
        }

        private Control CreateView(IViewModel viewModel)
        {
            Control view = GetDefaultView();

            try
            {
                uiSynchronization.Execute(() =>
                {
                    var viewType = viewLocator.Locate(viewModel);
                    view = Activator.CreateInstance(viewType) as Control;
                    view.DataContext = viewModel;

                    new ViewBehavior().Attach(view);
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return view;
        }

        private Window CreateWindow(IWindowViewModel viewModel)
        {
            Window window = GetDefaultWindow();

            try
            {
                uiSynchronization.Execute(() =>
                {
                    var windowType = viewLocator.Locate(viewModel);
                    window = Activator.CreateInstance(windowType) as Window;
                    window.DataContext = viewModel;

                    new ViewBehavior().Attach(window);
                    new WindowBehavior().Attach(window);
                    new WindowService(uiSynchronization).Attach(window);
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return window;
        }

        private Control GetDefaultView()
        {
            if (defaultView is null)
                uiSynchronization.Execute(() => defaultView = new DefaultView());
            return defaultView;
        }

        private Window GetDefaultWindow()
        {
            if (defaultWindow is null)
                uiSynchronization.Execute(() => defaultWindow = new DefaultWindow());
            return defaultWindow;
        }


    }
}
