using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SLStudio
{
    internal class ModuleRegister : IModuleRegister
    {
        private readonly IContainer container;
        private readonly Dictionary<Type, Type> services;
        private readonly Dictionary<Type, Type> workers;
        private readonly Dictionary<Type, Type> viewModels;
        private readonly Dictionary<Type, Type> views;
        private readonly Dictionary<Type, Type> windows;
        private readonly Dictionary<Type, Type> dialogs;
        private readonly List<IStudioTheme> themes;
        private readonly List<IStudioLanguage> languages;
        private readonly ResourceDictionary resources;

        public ModuleRegister(IContainer container)
        {
            this.container = container;
            services = new Dictionary<Type, Type>();
            workers = new Dictionary<Type, Type>();
            viewModels = new Dictionary<Type, Type>();
            views = new Dictionary<Type, Type>();
            windows = new Dictionary<Type, Type>();
            dialogs = new Dictionary<Type, Type>();
            themes = new List<IStudioTheme>();
            languages = new List<IStudioLanguage>();
            resources = new ResourceDictionary();

            Application.Current?.Resources.MergedDictionaries.Add(resources);
        }

        public IReadOnlyDictionary<Type, Type> Services => services;

        public IReadOnlyDictionary<Type, Type> Workers => workers;

        public IReadOnlyDictionary<Type, Type> ViewModels => viewModels;

        public IReadOnlyDictionary<Type, Type> Views => views;

        public IReadOnlyDictionary<Type, Type> Windows => windows;

        public IReadOnlyDictionary<Type, Type> Dialogs => dialogs;

        public IReadOnlyCollection<IStudioTheme> Themes => themes;

        public IReadOnlyCollection<IStudioLanguage> Languages => languages;

        void IModuleRegister.RegisterDisposable<TType>()
        {
            container.Register<TType>();
            SuppressDisposableWarning<TType>();
        }

        void IModuleRegister.RegisterDisposable<TService, TImplementation>()
        {
            container.Register<TService, TImplementation>();
            SuppressDisposableWarning<TService>();
        }

        void IModuleRegister.RegisterInstance<TService>(TService instance)
        {
            container.RegisterInstance(instance);
        }

        void IModuleRegister.RegisterSingleton<TConcrete>()
        {
            container.RegisterSingleton<TConcrete>();
        }

        void IModuleRegister.RegisterSingleton<TService, TImplementation>()
        {
            container.RegisterSingleton<TService, TImplementation>();
        }

        void IModuleRegister.RegisterService<TService, TImplementation>()
        {
            if (services.ContainsKey(typeof(TService)))
                return;

            container.RegisterSingleton<TImplementation>();
            container.RegisterSingleton<TService, TImplementation>();

            services.Add(typeof(TService), typeof(TImplementation));
        }

        void IModuleRegister.RegisterWorker<TWorker, TImplementation>()
        {
            if (workers.ContainsKey(typeof(TWorker)))
                return;

            container.RegisterSingleton<TImplementation>();
            container.RegisterSingleton<TWorker, TImplementation>();

            workers.Add(typeof(TWorker), typeof(TImplementation));
        }

        void IModuleRegister.RegisterViewModel<TViewModel, TImplementation>(Lifestyle lifestyle)
        {
            if (viewModels.ContainsKey(typeof(TViewModel)))
                return;

            container.Register<TImplementation>(lifestyle);
            container.Register<TViewModel, TImplementation>(lifestyle);

            SuppressDisposableWarning<TViewModel>();

            viewModels.Add(typeof(TViewModel), typeof(TImplementation));
        }

        void IModuleRegister.RegisterView<TView, TViewModel>()
        {
            if (views.ContainsKey(typeof(TView)))
                return;

            views.Add(typeof(TView), typeof(TViewModel));
        }

        void IModuleRegister.RegisterWindow<TWindow, TViewModel>()
        {
            if (windows.ContainsKey(typeof(TWindow)))
                return;

            windows.Add(typeof(TWindow), typeof(TViewModel));
        }

        void IModuleRegister.RegisterDialog<TDialog, TViewModel>()
        {
            if (dialogs.ContainsKey(typeof(TDialog)))
                return;

            dialogs.Add(typeof(TDialog), typeof(TViewModel));
        }

        void IModuleRegister.RegisterTheme<TTheme>(TTheme theme)
        {
            if (themes.Contains(theme))
                return;

            themes.Add(theme);
        }

        void IModuleRegister.RegisterLanguage<TLanguage>(TLanguage language)
        {
            if (languages.Contains(language))
                return;

            languages.Add(language);
        }

        void IModuleRegister.RegisterResource(Uri uri)
        {
            if (resources.MergedDictionaries.Any(resource => resource.Source == uri))
                return;

            var resource = new ResourceDictionary() { Source = uri };
            resources.MergedDictionaries.Add(resource);
        }

        private void SuppressDisposableWarning<TType>() where TType : class
        {
            var registration = container.GetRegistration<TType>().Registration;
            registration.SuppressDiagnosticWarning(SimpleInjector.Diagnostics.DiagnosticType.DisposableTransientComponent, "The caller should dispose");
        }
    }
}