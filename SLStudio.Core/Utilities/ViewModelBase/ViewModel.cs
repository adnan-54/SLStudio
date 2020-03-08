using Caliburn.Micro;
using DevExpress.Mvvm;
using SLStudio.Core.Utilities.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public class ViewModel : ViewModelBase, IViewModel
    {
        public static readonly object DefaultContext = new object();

        public ViewModel()
        {
            Activated = delegate { };
            AttemptingDeactivation = delegate { };
            Deactivated = delegate { };
            ViewAttached = delegate { };

            DisplayName = GetType().FullName;
            Views = new WeakValueDictionary<object, object>();
            IsNotifying = true;
        }

        public virtual event EventHandler<ActivationEventArgs> Activated;

        public virtual event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public virtual event EventHandler<DeactivationEventArgs> Deactivated;

        public event EventHandler<ViewAttachedEventArgs> ViewAttached;

        protected IDictionary<object, object> Views { get; }

        public virtual string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public virtual bool IsInitialized
        {
            get => GetProperty(() => IsInitialized);
            private set => SetProperty(() => IsInitialized, value);
        }

        public virtual bool IsActive
        {
            get => GetProperty(() => IsActive);
            private set => SetProperty(() => IsActive, value);
        }

        public virtual object Parent
        {
            get => GetProperty(() => Parent);
            set => SetProperty(() => Parent, value);
        }

        public virtual bool IsNotifying
        {
            get => GetProperty(() => IsNotifying);
            set => SetProperty(() => IsNotifying, value);
        }

        public async Task ActivateAsync(CancellationToken cancellationToken)
        {
            if (IsActive)
                return;

            var initialized = false;

            if (!IsInitialized)
            {
                await OnInitializeAsync(cancellationToken);
                IsInitialized = initialized = true;
            }

            await OnActivateAsync(cancellationToken);
            IsActive = true;

            Activated?.Invoke(this, new ActivationEventArgs
            {
                WasInitialized = initialized
            });
        }

        public void AttachView(object view, object context = null)
        {
            Views[context ?? DefaultContext] = view;

            var nonGeneratedView = PlatformProvider.Current.GetFirstNonGeneratedView(view);
            PlatformProvider.Current.ExecuteOnFirstLoad(nonGeneratedView, OnViewLoaded);
            OnViewAttached(nonGeneratedView, context);
            ViewAttached(this, new ViewAttachedEventArgs { View = nonGeneratedView, Context = context });

            if (!(this is IActivate activatable) || activatable.IsActive)
                PlatformProvider.Current.ExecuteOnLayoutUpdated(nonGeneratedView, OnViewReady);
            else
                AttachViewReadyOnActivated(activatable, nonGeneratedView);
        }

        private static void AttachViewReadyOnActivated(IActivate activatable, object nonGeneratedView)
        {
            var viewReference = new WeakReference(nonGeneratedView);
            void handler(object s, ActivationEventArgs e)
            {
                ((IActivate)s).Activated -= handler;
                var view = viewReference.Target;
                if (view != null)
                {
                    PlatformProvider.Current.ExecuteOnLayoutUpdated(view, ((ViewModel)s).OnViewReady);
                }
            }

            activatable.Activated += handler;
        }

        public virtual Task<bool> CanCloseAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task DeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (IsActive || IsInitialized && close)
            {
                AttemptingDeactivation?.Invoke(this, new DeactivationEventArgs
                {
                    WasClosed = close
                });

                await OnDeactivateAsync(close, cancellationToken);
                IsActive = false;

                Deactivated?.Invoke(this, new DeactivationEventArgs
                {
                    WasClosed = close
                });

                if (close)
                    Views.Clear();
            }
        }

        public virtual object GetView(object context = null)
        {
            Views.TryGetValue(context ?? DefaultContext, out object view);
            return view;
        }

        public virtual async Task TryCloseAsync(bool? dialogResult = null)
        {
            if (Parent is IConductor conductor)
                await conductor.CloseItemAsync(this, CancellationToken.None);

            var closeAction = PlatformProvider.Current.GetViewCloseAction(this, Views.Values, dialogResult);

            await Execute.OnUIThreadAsync(async () => await closeAction(CancellationToken.None));
        }

        protected virtual Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        protected virtual Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        protected virtual void OnViewLoaded(object view)
        {
        }

        protected virtual void OnViewAttached(object view, object context)
        {
        }

        protected virtual void OnViewReady(object view)
        {
        }

        protected virtual Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public virtual void NotifyOfPropertyChange(string propertyName)
        {
            if (IsNotifying)
            {
                if (PlatformProvider.Current.PropertyChangeNotificationsOnUIThread)
                    OnUIThread(() => RaisePropertiesChanged(propertyName));
                else
                    RaisePropertiesChanged(propertyName);
            }
        }

        public virtual void Refresh()
        {
            NotifyOfPropertyChange(string.Empty);
        }

        protected virtual void OnUIThread(System.Action action)
        {
            action.OnUIThread();
        }
    }
}