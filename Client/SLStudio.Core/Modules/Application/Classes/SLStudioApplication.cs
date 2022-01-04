using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SLStudio;

internal class SLStudioApplication : IApplication
{
    private static readonly ILogger logger = LogManager.GetLogger();

    private readonly Application application;
    private readonly SplashScreenView splashScreen;
    private bool isRunning;

    public SLStudioApplication(string[] args)
    {
        application = new();
        splashScreen = new();

        Args = args;
        Dispatcher = new SLStudioDispatcher(application.Dispatcher);

        application.MainWindow = splashScreen;
        application.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }

    public string[] Args { get; }

    public IDispatcher Dispatcher { get; }

    public IWindowViewModel MainWindow => GetMainWindow();

    public IWindowViewModel CurrentWindow => GetCurrentWindow();

    public IEnumerable<IWindowViewModel> Windows => GetWindows();

    public int Run()
    {
        if (isRunning)
            throw new InvalidOperationException("Application already running");
        isRunning = true;

        splashScreen.Show();
        application.Startup += OnApplicationStartup;
        application.Exit += OnApplicationExit;

        try
        {
            logger.Debug("Running application...");
            return application.Run();
        }
        catch (Exception ex)
        {
            logger.Exception(ex);
            LogManager.Default.Dump();
            throw;
        }
    }

    public void Shutdown(int exitCode)
    {
        logger.Debug($"Shutdown required with exit code '{exitCode}'");
        application.Shutdown(exitCode);
    }

    public bool TryFindResource(object resourceKey, out object resource)
    {
        resource = application.TryFindResource(resourceKey);
        return resource is not null;
    }

    private void OnApplicationExit(object sender, ExitEventArgs e)
    {
        LogManager.Default.Dump();
    }

    private IWindowViewModel GetMainWindow()
    {
        if (application.MainWindow.DataContext is IWindowViewModel viewModel)
            return viewModel;
        throw new ApplicationException("Could not find the application main window");
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private IWindowViewModel GetCurrentWindow()
    {
        var handle = GetActiveWindow();
        var hwndSource = HwndSource.FromHwnd(handle);
        var window = hwndSource?.RootVisual as Window;


        var windows = application.Windows.OfType<Window>();
        var window = windows.FirstOrDefault(w => w.IsActive);
        if (window is not null && window.DataContext is IWindowViewModel viewModel)
            return viewModel;
        throw new ApplicationException("Could not find the application current window");
    }

    private IEnumerable<IWindowViewModel> GetWindows()
    {
        var windows = application.Windows.OfType<Window>();
        var dataContexts = windows.Select(w => w.DataContext);

        foreach (var dataContext in dataContexts)
        {
            if (dataContext is IWindowViewModel viewModel)
                yield return viewModel;
        }
    }

    private async void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        var startup = new Startup(this, LogManager.Default);
        await startup.Configure();

        var shell = new Window();
        shell.Show();
        application.MainWindow = shell;
        splashScreen.Close();
        shell.Activate();
    }
}
