using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Core.Services.ToolManager
{
    class DefaultToolManager : IToolManager
    {
        private readonly Dictionary<Type, IToolItem> toolsCache;

        public DefaultToolManager(IMessenger messenger)
        {
            toolsCache = new();
            messenger.Register<ActiveWorkspaceChangedEvent>(this, UpdateTools);
        }

        public void Register<TService, TTool>(TTool tool) where TService : class where TTool : TService, IToolItem
        {
            var serviceType = typeof(TService);

            if (toolsCache.ContainsKey(serviceType))
                throw new InvalidOperationException($"Type {serviceType} is already registred");
            if (toolsCache.ContainsValue(tool))
                throw new InvalidOperationException($"Tool {tool} is already registred");

            toolsCache.Add(serviceType, tool);
        }

        public void Unregister(IToolItem tool)
        {
            if(!toolsCache.ContainsValue(tool))
                throw new InvalidOperationException($"{tool} is not registred");

            var key = toolsCache.First(kvp => kvp.Value == tool).Key;
            toolsCache.Remove(key);

        }

        private void UpdateTools(ActiveWorkspaceChangedEvent args)
        {
            if (args.NewItem is not IDocumentItem document)
                return;

            if (document.ToolContentProvider != null)
            {
                foreach(var kvp in toolsCache)
                {
                    var type = kvp.Key;
                    var tool = kvp.Value;
                    var content = document.ToolContentProvider.GetContent(type);
                    tool.SetContent(content);
                }
            }
            else
            {
                foreach (var tool in toolsCache.Values)
                    tool.SetContent(null);
            }
        }
    }
}
