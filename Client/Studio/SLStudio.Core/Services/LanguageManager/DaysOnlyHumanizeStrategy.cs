using Humanizer.DateTimeHumanizeStrategy;
using SLStudio.Core.Resources;
using System;
using System.Globalization;

namespace SLStudio.Core.Services.LanguageManager
{
    internal class DaysOnlyHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        private static readonly IDateTimeHumanizeStrategy DefaultStrategy = new DefaultDateTimeHumanizeStrategy();

        public string Humanize(DateTime input, DateTime comparisonBase, CultureInfo culture)
        {
            var inputDate = input.Date;
            var comparisonBaseDate = comparisonBase.Date;

            if (inputDate == comparisonBaseDate)
                return StudioResources.HumanizerDateTimeToday;

            return DefaultStrategy.Humanize(inputDate, comparisonBaseDate, culture);
        }
    }
}