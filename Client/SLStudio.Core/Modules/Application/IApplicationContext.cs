namespace SLStudio;

public interface IApplicationContext
{
    int Run();

    void AddSingleton();

    void AddTransient();
}
