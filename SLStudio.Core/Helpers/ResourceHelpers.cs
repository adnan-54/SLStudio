using System;
using System.Resources;

namespace SLStudio.Core
{
    public static class ResourceHelpers
    {
        public static string ResolveResouce(string name, Type resourceType)
        {
            if (resourceType == null)
                return name;
            var manager = new ResourceManager(resourceType);
            return manager.GetString(name) ?? name;
        }
    }
}