using System;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    public class ViewArea : ContentControl
    {
        private readonly IViewLocator viewLocator;

        public ViewArea()
        {
            viewLocator = IoC.Get<IViewLocator>();
        }

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ViewArea), new PropertyMetadata(null, new PropertyChangedCallback(ViewModelChanged)));

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue || e.NewValue == null || e.NewValue is not IViewModel viewModel)
                return;

            var control = d as ViewArea;
            control.OnViewModelChanged(viewModel);
        }

        private void OnViewModelChanged(IViewModel viewModel)
        {
            var viewModelType = viewModel.GetType();
            var viewType = viewLocator.LocateView(viewModelType);
            var view = Activator.CreateInstance(viewType) as UserControl;
            view.DataContext = viewModel;
            var behavior = new ViewBehavior();
            behavior.Attach(view);

            Content = view;
        }
    }
}