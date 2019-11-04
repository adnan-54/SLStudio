using Caliburn.Micro;
using SLStudio.Studio.Core.Modules.Toolbox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace SLStudio.Studio.Core.Modules.Toolbox.Services
{
    [Export(typeof(IToolboxService))]
    public class ToolboxService : IToolboxService
    {
        private readonly Dictionary<Type, IEnumerable<ToolboxItem>> _toolboxItems;

        public ToolboxService()
        {
            _toolboxItems = AssemblySource.Instance
                .SelectMany(x => x.GetTypes().Where(y => y.GetAttributes<ToolboxItemAttribute>(false).Any()))
                .Select(x =>
                {
                    var attribute = x.GetAttributes<ToolboxItemAttribute>(false).First();
                    return new ToolboxItem
                    {
                        DocumentType = attribute.DocumentType,
                        Name = attribute.Name,
                        Category = attribute.Category,
                        IconSource = attribute.IconSource != null ? new Uri(attribute.IconSource) : null,
                        ItemType = x,
                    };
                })
                .GroupBy(x => x.DocumentType)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
        }

        public IEnumerable<ToolboxItem> GetToolboxItems(Type documentType)
        {
            if (_toolboxItems.TryGetValue(documentType, out IEnumerable<ToolboxItem> result))
                return result;
            return new List<ToolboxItem>();
        }
    }
}