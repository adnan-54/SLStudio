using System.Windows.Data;
using Xceed.Wpf.AvalonDock.Converters;

namespace SLStudio.Studio.Core.Framework
{
    public static class Converters
    {
         public static readonly IValueConverter NullToDoNothingConverter = new NullToDoNothingConverter();
    }
}