using SimpleInjector;
using SLStudio.Core;

namespace SLStudio.Api
{
    internal class Module : StudioModule
    {
        protected override void Register(Container container)
        {
            container.RegisterService<IStudioClient, DefaultStudioClient>();
        }
    }
}