using Caliburn.Micro;
using SLStudio.Modules.MainWindow.ViewModels;
using System;

namespace SLStudio
{
    sealed class Module : IModule
    {
        private readonly SimpleContainer container;

        public Module(SimpleContainer container)
        {
            this.container = container;
        }

        public void Register()
        {
            container.PerRequest<MainWindowViewModel>();
        }
    }

    public interface IModule
    {
        void Register();
    }
}
