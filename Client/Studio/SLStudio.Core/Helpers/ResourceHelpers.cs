using System;
using System.Resources;

namespace SLStudio.Core
{
    public static class ResourceHelpers
    {
        public static string ResolveResouce(string name, Type resourceType, ResourceManager resourceManager = null)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            if (resourceType == null && resourceManager == null)
                return name;
            if (resourceManager == null)
                resourceManager = new ResourceManager(resourceType);
            return resourceManager.GetString(name) ?? name;
        }
    }
}