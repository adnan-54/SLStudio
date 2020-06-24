using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SLStudio.Core
{
    public static class ViewModelLocator
    {
        public static Type LocateViewModel(Type view)
        {
            var viewModelName = GetViewModelTypeName(view);

            var viewModels = view.Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.Name.Equals(viewModelName)).ToList();

            if (!viewModels.Any())
                throw new InvalidOperationException($"Cannot find view model from {view.FullName}");
            if (viewModels.Count > 1)
                throw new NotSupportedException($"{viewModels.Count} views with name {viewModelName} were found in the same assembly");

            return viewModels.FirstOrDefault();
        }

        private static string GetViewModelTypeName(Type viewModelType)
        {
            var name = viewModelType.Name;
            if (name.Contains("`"))
                name = name.Replace(name.Substring(name.IndexOf("`")), "");
            return Regex.Replace(name, @"View$", "ViewModel");
        }
    }
}