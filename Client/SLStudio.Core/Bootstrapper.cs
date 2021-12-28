namespace SLStudio;

public class Bootstrapper
{
    public static int Run(string[] args)
    {
        return new SLStudio(args).Run();
    }
}