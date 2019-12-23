using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SLStudio.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer container;

        public Bootstrapper()
        {
            container = new SimpleContainer();
            Initialize();
        }

        protected override void Configure()
        {
            container.Instance(container);
            container.Singleton<ISplashScreenService, SplashScreenService>();
            container.Singleton<IBootstrapperService, BootstrapperService>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            var bootstrapper = IoC.Get<IBootstrapperService>();
            bootstrapper.Initialize();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
