using DevExpress.Mvvm.UI.Interactivity;
using SLStudio.Core.Behaviors;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SLStudio.Core
{
    internal class DefaultWindowManager : IWindowManager
    {
        private readonly IObjectFactory objectFactory;

        public DefaultWindowManager(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public void ShowWindow(object model, Type viewType = null)
        {
            var window = InstantiateView(model, viewType);
            window.Show();
        }

        void IWindowManager.ShowWindow<T>()
        {
            ShowWindow(objectFactory.Create<T>());
        }

        public bool? ShowDialog(object model, Type viewType = null)
        {
            var window = InstantiateView(model, viewType);
            SetupModalOwner(window);

            return window.ShowDialog();
        }

        bool? IWindowManager.ShowDialog<T>()
        {
            return ShowDialog(objectFactory.Create<T>());
        }

        private Window InstantiateView(object model, Type viewType)
        {
            if (!(model is WindowViewModel viewModel))
                throw new NotSupportedException($"The {nameof(model)} needs to be a '{nameof(WindowViewModel)}");

            if (viewType == null)
                viewType = ViewLocator.LocateView(viewModel);

            var view = (Window)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            SetupTitle(view, viewModel);
            ApplyDefaultBehaviors(view);

            return view;
        }

        private void SetupTitle(Window view, WindowViewModel viewModel)
        {
            if (string.IsNullOrEmpty(view.Title))
            {
                if (string.IsNullOrEmpty(viewModel.DisplayName))
                    viewModel.DisplayName = viewModel.ToString();

                var binding = new Binding("DisplayName") { Mode = BindingMode.OneWay };
                view.SetBinding(Window.TitleProperty, binding);
            }
        }

        protected virtual void ApplyDefaultBehaviors(Window view)
        {
            var behaviors = Interaction.GetBehaviors(view);
            behaviors.Add(new CurrentWindowBehavior());
        }

        private void SetupModalOwner(Window window)
        {
            var owner = InferOwnerOf(window);
            if (owner != null)
                window.Owner = owner;
        }

        protected virtual Window InferOwnerOf(Window window)
        {
            var application = Application.Current;
            if (application == null)
                return null;

            var active = application.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
            active ??= (PresentationSource.FromVisual(application.MainWindow) == null ? null : application.MainWindow);
            return active == window ? null : active;
        }
    }
}