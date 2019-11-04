using SLStudio.Studio.Core.Modules.Toolbox.Models;
using System;

namespace SLStudio.Studio.Core.Modules.Toolbox.ViewModels
{
    public class ToolboxItemViewModel
    {
        public ToolboxItem Model { get; }

        public string Name
        {
            get { return Model.Name; }
        }

        public virtual string Category
        {
            get { return Model.Category; }
        }

        public virtual Uri IconSource
        {
            get { return Model.IconSource; }
        }

        public ToolboxItemViewModel(ToolboxItem model)
        {
            Model = model;
        }
    }
}