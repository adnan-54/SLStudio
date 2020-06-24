using Humanizer;
using SLStudio.Core.Modules.StartPage.Resources;
using SLStudio.Core.Modules.StartPage.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SLStudio.Core.Modules.StartPage.Converters
{
    internal class StartPageGroupFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecentFileViewModel recentFile)
            {
                if (recentFile.IsPinned)
                    return StartPageResources.Pinned;
                else
                    return recentFile.ModifiedDate.Humanize(false).ApplyCase(LetterCasing.Sentence);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}