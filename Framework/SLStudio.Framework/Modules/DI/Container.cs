using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SLStudio
{
    public sealed class Container : IContainer
    {
        private static Container instance;

        private readonly SimpleInjector.Container container;

        public Container()
        {
            container = new SimpleInjector.Container();
            container.Options.ResolveUnregisteredConcreteTypes = true;
            container.Options.AllowOverridingRegistrations = false;
        }
    }
}