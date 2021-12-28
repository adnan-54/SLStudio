namespace SLStudio;

internal class Startup
{
    public Startup(IApplication application)
    {

    }

    public async Task Configure()
    {
        //builder.UseDefaultServices();
        //builder.UseSplashScreen(splashScreen);
        //builder.ConfigureLogger();
        //builder.UseErrorHandler(errorHandler);
        //builder.UseModules("./SLStudio.*.dll");
        //builder.UseModules("./Modules/*.dll");
        //builder.LoadSettings();
        //builder.UseLanguage(language);
        //builder.UseTheme(theme);
        //builder.UseShell(shell);
        //var app = builder.Build();

        container.Verify();
    }
}
