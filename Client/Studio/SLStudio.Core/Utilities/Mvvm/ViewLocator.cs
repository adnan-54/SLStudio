using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SLStudio.Core
{
    public static class ViewLocator
    {
        public static Type LocateView(object viewModel)
        {
            var viewModelType = viewModel.GetType();
            var viewName = GetViewTypeName(viewModelType);

            var viewElements = viewModelType.Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => typeof(UIElement).IsAssignableFrom(t))
                .Where(t => t.Name.Equals(viewName)).ToList();

            if (!viewElements.Any())
                throw new InvalidOperationException($"Cannot find view from {viewModelType.FullName}");
            if (viewElements.Count > 1)
                throw new NotSupportedException($"{viewElements.Count} views with name {viewName} were found in the same assembly");

            return viewElements.FirstOrDefault();
        }

        private static string GetViewTypeName(Type viewModelType)
        {
            var name = viewModelType.Name;
            if (name.Contains("`"))
                name = name.Replace(name[name.IndexOf("`")..], "");
            return Regex.Replace(name, @"ViewModel$", "View");
        }
    }
}