using DevExpress.Mvvm;
using System;
using System.Linq;

namespace SLStudio.Core.Modules.NewFile.Models
{
    internal class CategoryModel : BindableBase
    {
        public CategoryModel(string path, string displayName = null)
        {
            Path = path;
            if (string.IsNullOrEmpty(displayName))
                DisplayName = Path.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
            else
                DisplayName = displayName;

            Children = new BindableCollection<CategoryModel>();
        }

        public string DisplayName { get; }

        public string Path { get; }

        public IObservableCollection<CategoryModel> Children { get; }

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public bool Match(NewFileModel file)
        {
            return !string.IsNullOrEmpty(file.Category) && file.Category.StartsWith(Path, StringComparison.OrdinalIgnoreCase);
        }
    }
}