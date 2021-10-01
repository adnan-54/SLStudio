namespace SLStudio
{
    [StudioModule]
    internal class Module : StudioModule
    {
        protected override void Register(IModuleRegister register)
        {
            register.RegisterService<IApplicationInfo, ApplicationInfo>();
            register.RegisterService<IAssemblyLoader, AssemblyLoader>();
            register.RegisterService<IMessenger, Messenger>();
            register.RegisterService<IModuleContainerFactory, ModuleContainerFactory>();
            register.RegisterService<IModuleLoader, ModuleLoader>();
            register.RegisterService<IObjectFactory, ObjectFactory>();
            register.RegisterService<ISplashScreen, SplashScreen>();
            register.RegisterService<ITempStorage, TempStorage>();
            register.RegisterService<IUiSynchronization, UiSynchronization>();
        }
    }
}