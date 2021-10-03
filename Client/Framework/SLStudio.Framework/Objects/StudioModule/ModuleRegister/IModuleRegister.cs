using System;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    public interface IModuleRegister : IHaveModuleInfos
    {
        void RegisterDisposable<TType>()
            where TType : class;

        void RegisterDisposable<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void RegisterInstance<TService>(TService instance)
            where TService : class;

        public void RegisterSingleton<TConcrete>()
            where TConcrete : class;

        public void RegisterSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void RegisterService<TService, TImplementation>()
            where TService : class, IStudioService
            where TImplementation : StudioService, TService;

        void RegisterWorker<TWorker, TImplementation>()
            where TWorker : class, IStudioService
            where TImplementation : StudioWorker, TWorker;

        void RegisterViewModel<TViewModel, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient)
            where TViewModel : class, IStudioViewModel
            where TImplementation : StudioViewModel, TViewModel;

        void RegisterView<TView, TViewModel>()
            where TView : UserControl
            where TViewModel : class, IStudioViewModel;

        void RegisterWindow<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : class, IWindowViewModel;

        void RegisterDialog<TDialog, TViewModel>()
            where TDialog : Window
            where TViewModel : class, IDialogViewModel;

        void RegisterTheme<TTheme>(TTheme theme)
            where TTheme : class, IStudioTheme;

        void RegisterLanguage<TLanguage>(TLanguage language)
            where TLanguage : class, IStudioLanguage;

        void RegisterResource(Uri uri);
    }
}