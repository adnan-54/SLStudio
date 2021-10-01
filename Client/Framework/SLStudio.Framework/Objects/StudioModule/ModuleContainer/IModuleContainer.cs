namespace SLStudio
{
    public interface IModuleContainer : IHaveModuleInfos
    {
        IStudioModule Module { get; }
        IModuleRegister Register { get; }
        IModuleScheduler Scheduler { get; }
    }
}