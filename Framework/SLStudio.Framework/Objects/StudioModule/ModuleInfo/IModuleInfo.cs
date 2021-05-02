namespace SLStudio
{
    public interface IModuleInfo : IHaveModuleInfos
    {
        IStudioModule Module { get; }
    }
}