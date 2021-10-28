using System;

namespace SLStudio
{
    internal class ViewModelFactory : IViewModelFactory
    {
        private readonly IViewModelResolver viewModelResolver;
        private readonly IObjectFactory objectFactory;

        public ViewModelFactory(IViewModelResolver viewModelResolver, IObjectFactory objectFactory)
        {
            this.viewModelResolver = viewModelResolver;
            this.objectFactory = objectFactory;
        }

        public IViewModel Create(Type viewModelType)
        {
            if (!viewModelType.IsAssignableTo(typeof(IViewModel)))
                throw new InvalidOperationException($"The view model type must implement '{nameof(IViewModel)}'");

            viewModelType = viewModelResolver.Resolve(viewModelType);
            return objectFactory.Create<IViewModel>(viewModelType);
        }

        TViewModel IViewModelFactory.Create<TViewModel>()
        {
            return Create(typeof(TViewModel)) as TViewModel;
        }
    }
}
