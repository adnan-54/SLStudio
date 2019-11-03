using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Services
{
    [Export(typeof(IServiceProvider))]
    public class ServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return IoC.GetInstance(serviceType, null);
        }
    }
}