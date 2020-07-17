using SimpleInjector;
using SLStudio.Core;
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

        protected override Task Run(IObjectFactory objectFactory)
        {
            var shell = objectFactory.Create<IShell>();
            var testViewModel = objectFactory.Create<ITest>();
            shell?.OpenPanel(testViewModel);

            return Task.CompletedTask;
        }
    }
}