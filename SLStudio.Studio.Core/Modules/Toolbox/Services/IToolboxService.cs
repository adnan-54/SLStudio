using SLStudio.Studio.Core.Modules.Toolbox.Models;
using System;
using System.Collections.Generic;

namespace SLStudio.Studio.Core.Modules.Toolbox.Services
{
    public interface IToolboxService
    {
        IEnumerable<ToolboxItem> GetToolboxItems(Type documentType);
    }
}