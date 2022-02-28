namespace SLStudio;

public interface ICommandStorage
{
    IStudioCommand GetFromKey(string key);
}
