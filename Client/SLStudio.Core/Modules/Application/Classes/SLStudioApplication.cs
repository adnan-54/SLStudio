using System.Runtime.InteropServices;
using System.Windows.Interop;
using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

internal class SLStudioApplication : IApplication
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
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

        try
        {
            logger.Debug("Running application...");
            return application.Run();
        }
        catch (Exception ex)
        {
            logger.Fatal("A fatal exception occurred. Application runtime terminating.");
            logger.Exception(ex);
            throw;
        }
        finally
        {
            LogManager.Default.Dump();
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

    private IWindowViewModel GetMainWindow()
    {
        if (application.MainWindow.DataContext is IWindowViewModel viewModel)
            return viewModel;
        throw new ApplicationException("Could not find the application main window");
    }

    private static IWindowViewModel GetCurrentWindow()
    {
        try
        {
            var handle = GetActiveWindow();
            var hwndSource = HwndSource.FromHwnd(handle);
            var window = hwndSource?.RootVisual as Window;

            if (window is not null && window.DataContext is IWindowViewModel viewModel)
                return viewModel;
        }
        catch (Exception ex)
        {
            logger.Exception(ex);
        }

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
        var host = new ContainerHost(this);
        var startup = new Startup(host);

        await Task.Run(startup.Configure);

        var windowManager = host.ServiceProvider.GetService<IWindowManager>()!;
        var shell = windowManager.ShowModal<IShell>();
        var window = windowManager.Windows.First(w => w.ViewModel == shell).Window as StudioWindow;
        application.MainWindow = window;
        splashScreen.Close();
    }
}
