using Humanizer.DateTimeHumanizeStrategy;
using SLStudio.Core.Resources.Language;
using System;
using System.Globalization;

namespace SLStudio.Core.Humanizer
{
    internal class DaysOnlyHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        private static readonly IDateTimeHumanizeStrategy DefaultStrategy = new DefaultDateTimeHumanizeStrategy();

        public string Humanize(DateTime input, DateTime comparisonBase, CultureInfo culture)
        {
            var inputDate = input.Date;
            var comparisonBaseDate = comparisonBase.Date;

            if (inputDate == comparisonBaseDate)
                return Language.HumanizerDateTimeToday;

            return DefaultStrategy.Humanize(inputDate, comparisonBaseDate, culture);
        }
    }
}