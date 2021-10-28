using System;
using System.Windows.Controls;

namespace SLStudio
{
    internal interface IViewFactory
    {
        TView Create<TView>(IViewModel viewModel)
            where TView : Control;

        TView Create<TView>(Type viewModelType)
            where TView : Control;

        TView Create<TView, TViewModel>()
            where TView : Control
            where TViewModel : class, IViewModel;
    }
}
