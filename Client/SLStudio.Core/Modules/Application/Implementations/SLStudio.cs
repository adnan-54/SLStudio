namespace SLStudio;

internal class SLStudio : IApplication
{
    private static readonly ILogger logger = LogManager.GetLogger();

    private readonly Application application;
    private readonly SplashScreenView splashScreen;
    private bool isRunning;

    public SLStudio(string[] args)
    {
        application = new();
        splashScreen = new();

        Args = args;

        application.MainWindow = splashScreen;
        application.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }

    public string[] Args { get; }

    public IDispatcher Dispatcher { get; }

    public IWindowViewModel MainWindow { get; }

    public IWindowViewModel CurrentWindow { get; }

    public IEnumerable<IWindowViewModel> Windows { get; }

    public int Run()
    {
        if (isRunning)
            throw new InvalidOperationException("Application already running");
        isRunning = true;

        splashScreen.Show();

        application.Startup += OnApplicationStartup;
        try
        {
            return application.Run();
        }
        catch (Exception ex)
        {
            logger.Exception(ex);
            throw;
        }
    }

    public void Shutdown(int exitCode)
    {
        throw new NotImplementedException();
    }

    public bool TryFindResource(object resourceKey, out object resource)
    {
        throw new NotImplementedException();
    }

    private async void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        var startup = new Startup(this);
        await startup.Configure();

        var shell = new Window();
        shell.Show();
        application.MainWindow = shell;
        splashScreen.Close();
        shell.Activate();
    }
}
