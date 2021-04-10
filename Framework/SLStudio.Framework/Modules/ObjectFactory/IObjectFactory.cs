using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IObjectFactory : IStudioService
    {
        TService Create<TService>()
            where TService : class;

        object Create(Type serviceType);

        TService Create<TService>(Type serviceType)
            where TService : class;
    }
}