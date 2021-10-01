namespace SLStudio
{
    internal interface IModuleContainerFactory : IStudioService
    {
        IModuleContainer CreateContainerFor(IStudioModule module);
    }
}