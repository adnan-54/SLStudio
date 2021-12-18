namespace SLStudio;

public class Bootstrapper
{
    private static readonly Bootstrapper bootstrapper = new();

    public Bootstrapper()
    {
        context = new ApplicationContext();
        application = new SLStudio();
    }

    private int RunInternal(params string[] args)
    {
        return application.Run();
    }


    public static int Run(params string[] args)
    {
        return bootstrapper.RunInternal(args);
    }
}