namespace SLStudio;

public partial class View
{
    public static readonly DependencyProperty ViewModelProperty;

    static View()
    {
        ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(IViewModel), typeof(View), new PropertyMetadata(default, OnViewModelChanged));
    }

    private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not View view)
            return;

        if (e.NewValue is not IViewModel viewModel)
        {
            view.Content = null;
            return;
        }

        var viewFactory = IoC.Get<IViewFactory>();
        var viewContent = viewFactory.CreateFromViewModel(viewModel.GetType());
        viewContent.PerformAction(control => control.DataContext = viewModel);
        view.Content = viewContent;
    }
}