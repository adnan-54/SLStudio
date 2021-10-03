using System.ComponentModel;

namespace SLStudio.Core.Modules.NewFile.Models
{
    internal class SortModeModel
    {
        internal SortModeModel(string displayName, SortDescription sortDescription)
        {
            DisplayName = displayName;
            SortDescription = sortDescription;
        }

        public string DisplayName { get; }

        public SortDescription SortDescription { get; }
    }
}