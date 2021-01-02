using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core.Menus
{
    internal class DefaultMenuLookup : IMenuLookup
    {
        private readonly IObjectFactory objectFactory;

        public DefaultMenuLookup(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public IEnumerable<IMenuConfiguration> Lookup()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var items = assemblies.SelectMany(a => a.DefinedTypes).Where(t => !t.IsAbstract && !t.IsAbstract && typeof(IMenuConfiguration).IsAssignableFrom(t));
            return items.Select(c => objectFactory.Create(c) as IMenuConfiguration);
        }
    }
}