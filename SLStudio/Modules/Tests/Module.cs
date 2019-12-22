using Caliburn.Micro;
using SLStudio.Core;
using System.Windows;

namespace SLStudio.Modules.Tests
{
    class Module : IModule
    {
        public bool ShouldBeLoaded => true;

        public string ModuleName => "Test Module";

        public string ModuleDescrition => "Used for tests";

        public void Register(SimpleContainer container)
        {

        }
    }
}
