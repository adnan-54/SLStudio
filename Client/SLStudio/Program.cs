namespace SLStudio;

internal static class Program
{
    [STAThread]
    private static int Main(params string[] args)
    {
        return Bootstrapper.Run();
    }
}