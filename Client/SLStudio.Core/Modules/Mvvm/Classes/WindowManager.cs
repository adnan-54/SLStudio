namespace SLStudio;

internal class WindowManager : IWindowManager
{
    private readonly IApplication application;
    private readonly IViewModelFactory viewModelFactory;
    private readonly IViewFactory viewFactory;
    private readonly IList<IWindowInfo> windows;

    public WindowManager(IApplication application, IViewModelFactory viewModelFactory, IViewFactory viewFactory)
    {
        this.application = application;
        this.viewModelFactory = viewModelFactory;
        this.viewFactory = viewFactory;
        windows = new List<IWindowInfo>();
    }

    public IEnumerable<IWindowInfo> Windows => windows;

    public IWindowInfo? MainWindow => windows.FirstOrDefault(w => w.ViewModel == application.MainWindow);

    public IWindowInfo? CurrentWindow => windows.FirstOrDefault(w => w.ViewModel == application.CurrentWindow);

    TViewModel IWindowManager.ShowModal<TViewModel>()
    {
        var viewModel = viewModelFactory.Create<TViewModel>();
        return ((IWindowManager)this).ShowModal(viewModel);
    }

    TViewModel IWindowManager.ShowModal<TViewModel>(TViewModel viewModel)
    {
        var window = CreateWindow(viewModel, true);
        window.PerformAction(control =>
        {
            if (control is Window window)
                window.Show();
        });
        return viewModel;
    }

    TViewModel IWindowManager.ShowDialog<TViewModel>()
    {
        var viewModel = viewModelFactory.Create<TViewModel>();
        return ((IWindowManager)this).ShowDialog(viewModel);
    }

    TViewModel IWindowManager.ShowDialog<TViewModel>(TViewModel viewModel)
    {
        var window = CreateWindow(viewModel, false);
        window.PerformAction(control =>
        {
            if (control is Window window)
                window.ShowDialog();
        });
        return viewModel;
    }

    void IWindowManager.Activate<TViewModel>(TViewModel viewModel)
    {
        var window = GetWindow(viewModel);
        window.PerformAction(control =>
        {
            if (control is not Window window)
                return;

            if (window.WindowState == WindowState.Minimized)
                SystemCommands.RestoreWindow(window);

            window.Activate();
            window.Focus();
        });
    }

    void IWindowManager.Maximize<TViewModel>(TViewModel viewModel)
    {
        var window = GetWindow(viewModel);
        window.PerformAction(control =>
        {
            if (control is Window window)
                SystemCommands.MaximizeWindow(window);
        });
    }

    void IWindowManager.Restore<TViewModel>(TViewModel viewModel)
    {
        var window = GetWindow(viewModel);
        window.PerformAction(control =>
        {
            if (control is Window window)
                SystemCommands.RestoreWindow(window);
        });
    }

    void IWindowManager.Minimize<TViewModel>(TViewModel viewModel)
    {
        var window = GetWindow(viewModel);
        window.PerformAction(control =>
        {
            if (control is Window window)
                SystemCommands.MinimizeWindow(window);
        });
    }

    void IWindowManager.Close<TViewModel>(TViewModel viewModel)
    {
        var window = GetWindow(viewModel);
        window.PerformAction(control =>
        {
            if (control is Window window)
                window.Close();
        });
    }

    private IWindowView CreateWindow<TViewModel>(TViewModel viewModel, bool isModal)
        where TViewModel : class, IWindowViewModel
    {
        if (viewFactory.CreateFromViewModel<TViewModel>() is not ModalWindow view)
            throw new InvalidOperationException($"Could not find any window for {nameof(TViewModel)}");

        view.DataContext = viewModel;
        windows.Add(new WindowInfo(view, viewModel, isModal));
        return view;
    }

    private IWindowView GetWindow<TViewModel>(TViewModel viewModel)
        where TViewModel : class, IWindowViewModel
    {
        var info = windows.FirstOrDefault(i => i.ViewModel == viewModel);
        if (info is null)
            throw new InvalidOperationException($"Could not find view model {nameof(TViewModel)}");
        return info.Window;
    }
}
