using SimpleInjector;
using SLStudio.Core;

namespace SLStudio.Api
{
    internal class Module : ModuleBase
    {
        protected override void Register(Container container)
        {
            container.RegisterService<IStudioClient, DefaultStudioClient>();
        }
    }
}