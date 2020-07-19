using SimpleInjector;
using SLStudio.Core;
using SLStudio.Core.Menus;
using SLStudio.Tests.Menus;
using SLStudio.Tests.Modules.Tests.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.Tests
{
    internal class Module : ModuleBase
    {
        public override string Name => "SLStudio Tests";

        protected override void Register(Container container)
        {
            container.RegisterDisposable<ITest, TestViewModel>();
        }
    }
}